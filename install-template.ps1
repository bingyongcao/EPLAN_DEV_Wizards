$ErrorActionPreference = 'Stop'

$wizardDll = 'D:\repos\EPLAN_API_TUTORIAL\EPLAN_ADDIN_TEMPLATE.Wizard\bin\Release\EPLAN_ADDIN_TEMPLATE.Wizard.dll'
$publicAssembliesDir = 'C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\PublicAssemblies'
$templateZipSource = 'C:\Users\cby\Documents\Visual Studio 18\My Exported Templates\EPLAN_ADDIN_TEMPLATE.zip'
$templateZipDestination = 'C:\Users\cby\Documents\Visual Studio 18\Templates\ProjectTemplates\EPLAN_ADDIN_TEMPLATE.zip'

if (-not (Test-Path -LiteralPath $wizardDll -PathType Leaf)) {
    throw "Wizard DLL not found: $wizardDll`nBuild EPLAN_ADDIN_TEMPLATE.Wizard in Release first."
}

if (-not (Test-Path -LiteralPath $publicAssembliesDir -PathType Container)) {
    throw "Visual Studio PublicAssemblies folder not found: $publicAssembliesDir"
}

if (-not (Test-Path -LiteralPath $templateZipSource -PathType Leaf)) {
    throw "Template zip not found: $templateZipSource"
}

$templateZipDirectory = Split-Path -Path $templateZipDestination -Parent
New-Item -ItemType Directory -Path $templateZipDirectory -Force | Out-Null

Copy-Item -LiteralPath $wizardDll -Destination $publicAssembliesDir -Force
Copy-Item -LiteralPath $templateZipSource -Destination $templateZipDestination -Force

Write-Host "Copied wizard DLL to: $publicAssembliesDir"
Write-Host "Copied template zip to: $templateZipDestination"
Write-Host 'Done. Restart Visual Studio if it was open.'
