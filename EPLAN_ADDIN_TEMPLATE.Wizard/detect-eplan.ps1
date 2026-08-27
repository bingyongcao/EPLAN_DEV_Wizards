[CmdletBinding()]
param(
    [string[]]$SearchRoot,
    [switch]$AsJson
)

$ErrorActionPreference = 'Stop'
$roots = [System.Collections.Generic.List[object]]::new()
$seenRoots = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

function Add-SearchRoot {
    param(
        [string]$Path,
        [string]$Source,
        [bool]$AllowDriveRoot = $false
    )

    if ([string]::IsNullOrWhiteSpace($Path)) { return }

    $candidate = [Environment]::ExpandEnvironmentVariables($Path.Trim().Trim('"'))
    if (-not (Test-Path -LiteralPath $candidate -PathType Container)) { return }

    $resolved = (Resolve-Path -LiteralPath $candidate).Path
    $driveRoot = [System.IO.Path]::GetPathRoot($resolved)
    if (-not $AllowDriveRoot -and
        $resolved.TrimEnd('\') -eq $driveRoot.TrimEnd('\')) {
        return
    }

    if ($seenRoots.Add($resolved)) {
        $roots.Add([pscustomobject]@{
            Path = $resolved
            Source = $Source
        })
    }
}

function ConvertFrom-EplanDirectoryValue {
    param([object]$Value)

    if ($Value -isnot [string] -or [string]::IsNullOrWhiteSpace($Value)) { return $null }

    $directory = $Value.Trim()
    $separator = $directory.IndexOf('=')
    if ($separator -ge 0) {
        $directory = $directory.Substring($separator + 1)
    }
    return ($directory -replace '\\\?\-r$', '').TrimEnd('\')
}

function Add-EplanRegistryRoots {
    foreach ($platformRoot in @(
        'HKLM:\SOFTWARE\EPLAN\Eplan W3\Platform',
        'HKLM:\SOFTWARE\WOW6432Node\EPLAN\Eplan W3\Platform'
    )) {
        if (-not (Test-Path -LiteralPath $platformRoot)) { continue }

        foreach ($versionKey in Get-ChildItem -LiteralPath $platformRoot -ErrorAction SilentlyContinue) {
            $source = "Registry:EPLAN Platform $($versionKey.PSChildName)"
            $systemDirectories = Join-Path $versionKey.PSPath 'SystemDirectories'
            if (Test-Path -LiteralPath $systemDirectories) {
                $root = (Get-ItemProperty -LiteralPath $systemDirectories -ErrorAction SilentlyContinue).Root
                Add-SearchRoot -Path $root -Source $source
            }

            $installDirectories = Join-Path $versionKey.PSPath 'InstallInfo\Directories'
            if (Test-Path -LiteralPath $installDirectories) {
                $binaries = (Get-ItemProperty -LiteralPath $installDirectories -ErrorAction SilentlyContinue).Binaries
                Add-SearchRoot -Path (ConvertFrom-EplanDirectoryValue $binaries) -Source $source
            }
        }
    }



    if ($roots.Count -gt 0) { return }

    foreach ($uninstallRoot in @(
        'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall',
        'HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall',
        'HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall'
    )) {
        if (-not (Test-Path -LiteralPath $uninstallRoot)) { continue }

        foreach ($key in Get-ChildItem -LiteralPath $uninstallRoot -ErrorAction SilentlyContinue) {
            $entry = Get-ItemProperty -LiteralPath $key.PSPath -ErrorAction SilentlyContinue
            if ($entry.DisplayName -notmatch '^Eplan (Platform|Electric P8)(\s|$)') { continue }

            $source = "Registry:Uninstall $($entry.DisplayName)"
            Add-SearchRoot -Path $entry.InstallLocation -Source $source

            if ($entry.DisplayName -match '^Eplan Electric P8' -and $entry.InstallLocation) {
                $productRoot = Split-Path -Parent $entry.InstallLocation.TrimEnd('\')
                $vendorRoot = if ($productRoot) { Split-Path -Parent $productRoot } else { $null }
                Add-SearchRoot -Path $vendorRoot -Source $source
            }
        }
    }
}

if ($SearchRoot) {
    foreach ($root in $SearchRoot) {
        Add-SearchRoot -Path $root -Source 'Explicit' -AllowDriveRoot $true
    }
} else {
    Add-EplanRegistryRoots

    if ($roots.Count -eq 0) {
        foreach ($base in @(
            [Environment]::GetFolderPath('ProgramFiles'),
            [Environment]::GetFolderPath('ProgramFilesX86')
        )) {
            if (-not [string]::IsNullOrWhiteSpace($base)) {
                Add-SearchRoot -Path (Join-Path $base 'EPLAN') -Source 'StandardDirectory'
            }
        }
    }
}

$seen = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
$installations = [System.Collections.Generic.List[object]]::new()

foreach ($root in $roots) {
    $baseAssemblies = Get-ChildItem -LiteralPath $root.Path -Filter 'Eplan.EplApi.Base*.dll' -File -Recurse -ErrorAction SilentlyContinue |
        Where-Object Name -in @('Eplan.EplApi.Base.dll', 'Eplan.EplApi.Baseu.dll')
    foreach ($assembly in $baseAssemblies) {
        if (-not $seen.Add($assembly.FullName)) { continue }

        $directory = $assembly.Directory.FullName
        $isUnified = $assembly.Name -eq 'Eplan.EplApi.Baseu.dll'
        $applicationFrameworkName = if ($isUnified) { 'Eplan.EplApi.AFu.dll' } else { 'Eplan.EplApi.ApplicationFramework.dll' }
        $applicationFramework = Join-Path $directory $applicationFrameworkName
        $xmlDocumentation = [System.IO.Path]::ChangeExtension($assembly.FullName, '.xml')
        $versionInfo = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($assembly.FullName)

        $installations.Add([pscustomobject]@{
            SearchRoot = $root.Path
            DiscoverySource = $root.Source
            AssemblyDirectory = $directory
            BaseAssembly = $assembly.FullName
            AssemblyProfile = if ($isUnified) { 'Unified' } else { 'Legacy' }
            ApplicationFrameworkAssembly = if (Test-Path -LiteralPath $applicationFramework) { $applicationFramework } else { $null }
            XmlDocumentation = if (Test-Path -LiteralPath $xmlDocumentation) { $xmlDocumentation } else { $null }
            FileVersion = $versionInfo.FileVersion
            ProductVersion = $versionInfo.ProductVersion
        })
    }
}

$ordered = @($installations | Sort-Object FileVersion, AssemblyDirectory -Descending)
if ($AsJson) {
    ConvertTo-Json -InputObject $ordered -Depth 4
} else {
    $ordered
}
