param (
    $assemblies,
    [string]$rootDirectory,
    [string]$accessToken,
    [string]$baseUrl = "https://api.eplan.com/eadn-signing/v1.0",
    [string]$comment,
    [string]$destinationPath,
    [switch]$deleteAfterwards
)

# Example command line for running this script (not China):
# powershell -ExecutionPolicy Bypass -file "<YourFolderName>\PostBuildScript.ps1" -comment "This is a test comment from $(USERNAME)" -accessToken "<YourPATforEADNSigningService>" -assemblies "<SourceFolderWithPreSignedFiles>\$(TargetFileName)" -destinationPath "<TargetFolder>" -deleteAfterwards

# Example command line for running upload on multiple file in a folder and possible subdirectories (not China)
# powershell -ExecutionPolicy Bypass -file "<YourFolderName>\PostBuildScript.ps1" -comment "This is a test comment from $(USERNAME)" -accessToken "<YourPATforEADNSigningService>" -assemblies "AssemblyA.dll,en-US\AssemblyB.dll,ExampleDir\SampleDir\AssemblyC.dll" -rootDirectory "<path to directory of assemblies>" -destinationPath "<TargetFolder>" -deleteAfterwards

# Example command line for running this script (China only):
# powershell -ExecutionPolicy Bypass -file "<YourFolderName>\PostBuildScript.ps1" -baseUrl "https://api.eplan.com.cn/eadn-signing/v1.0" -comment "This is a test comment from $(USERNAME)" -accessToken "<YourPATforEADNSigningService>" -assemblies "<SourceFolderWithPreSignedFiles>\$(TargetFileName)" -destinationPath "<TargetFolder>" -deleteAfterwards

# Example command line for running upload on multiple file in a folder and possible subdirectories (China only)
# powershell -ExecutionPolicy Bypass -file "<YourFolderName>\PostBuildScript.ps1" -baseUrl "https://api.eplan.com.cn/eadn-signing/v1.0" -comment "This is a test comment from $(USERNAME)" -accessToken "<YourPATforEADNSigningService>" -assemblies "AssemblyA.dll,en-US\AssemblyB.dll,ExampleDir\SampleDir\AssemblyC.dll" -rootDirectory "<path to directory of assemblies>" -destinationPath "<TargetFolder>" -deleteAfterwards


# Changelog:
# 1.8 Trim leading backslash only if assembly path is not UNC path
# 1.7 Improved error output in case of upload failure
# 1.6 Updated example calls for Chinese environment due to domain change, improved message appearance in output-console
# 1.5 Add example calls for Chinese environment
# 1.4 Add functionaility to upload files from a folder with subdirectories
# 1.3 Fix function order issue
# 1.2 Check assemblies for valid file paths (no comma, no leading/traling space, no | \0 \t) and existence
# 1.1 Give assemblies as a comma separated list of paths
# 1.0 Initial release

$ErrorActionPreference = 'Stop'

Add-Type -AssemblyName 'System.Net.Http' # Load .NET Http library

function EnsureValidAssemblyPath {
    param([string]$assemblyPath)

    if ($assemblyPath.StartsWith(' ') -or $assemblyPath.EndsWith(' ')) { return $false }
    return $assemblyPath.IndexOfAny([System.IO.Path]::GetInvalidPathChars()) -eq -1
}

function EnsureAssemblyPathExists {
    param([string]$assemblyPath)

    return Test-Path -Path $assemblyPath
}


if (![string]::IsNullOrWhitespace($rootDirectory)) {
    if (!$rootDirectory.EndsWith("\")) {
        $rootDirectory = $rootDirectory + "\"
    }
    Write-Host "Root directory: $($rootDirectory)"
}

$assemblies = $assemblies.Split(",")
$assemblies = $assemblies | ForEach-Object { if (-not(([System.Uri]$_).IsUnc)) { $rootDirectory + $_.TrimStart("\") } else { $_ } }
$assemblies | ForEach-Object {
  if (-not(EnsureValidAssemblyPath -assemblyPath $_)) {
    Write-Error "Assembly path '$_' contains invalid characters."
  }
  if (-not(EnsureAssemblyPathExists -assemblyPath $_)) {
    Write-Error "Assembly path '$_' does not exist."
  }
}

function Await-Task {
    param (
        [Parameter(ValueFromPipeline=$true, Mandatory=$true)]
        $task
    )

    process {
        while (-not $task.AsyncWaitHandle.WaitOne(200)) { }
        $task.GetAwaiter().GetResult();
    }
}

function UploadFiles($filePaths)
{
    Write-Host "Preparing file upload"
    $url = "$baseUrl/assemblies";
    $client = New-Object System.Net.Http.HttpClient;
    $content = New-Object System.Net.Http.MultipartFormDataContent;
    
    if (-not [string]::IsNullOrWhitespace($comment)) # Add comment if given
    {
        $commentSection = New-Object System.Net.Http.StringContent($comment)
        $content.Add($commentSection, "comment");
    }

    $i = 0;
    foreach ($filePath in $filePaths)
    {
        $fieldName = "files[$i]";
        
        if ($rootDirectory) {
            $fileName = $filePath.Replace($rootDirectory, "")
        } else {
            $fileName = Split-Path $filePath -Leaf;
        }
        Write-Host "Adding file '$filePath' with file name/path: '$fileName'"
        $fileStream = [System.IO.File]::OpenRead($filePath);
        $fileContent = New-Object System.Net.Http.StreamContent($fileStream);
        $content.Add($fileContent, $fieldName, $fileName);
        $i++;
    }

    $resultContent = $null;
    try {
        Write-Host "Trying to upload files ... "  -NoNewline

        $client.DefaultRequestHeaders.Authorization = New-Object System.Net.Http.Headers.AuthenticationHeaderValue -ArgumentList "Bearer",$accessToken;
        $result = $client.PostAsync($url, $content) | Await-Task;
        if (-not $result.IsSuccessStatusCode)
        {
            Write-Error "Failed to upload assembly: $($result.StatusCode)"
            $x = $result.EnsureSuccessStatusCode();
        }
        $resultContent = $result.Content.ReadAsStringAsync() | Await-Task | ConvertFrom-Json;
        
        Write-Host "Done" -ForegroundColor Green

        $resultContent.links
    }
    catch  {
        Write-Host "Upload failed:" -ForegroundColor Red
        if (-not $resultContent) {
            if ($_.Exception.InnerException) {
                Write-Error $_.Exception.InnerException
            } else {
                Write-Error $_
            }
        } else {
            $resultContent | ConvertTo-Json | Write-Error
        }
    }
}

function CheckStatus($statusUrl)
{
    $success = $true; # Signed successfully
    $finished = $false; # Processing finished
    $tries = 1; #  Amount of status checks executed
    $maxTries = 100; # Maximum amount of calling the status check endpoint
    do {
        Write-Host "Checking status ($("{0:d3}" -f $tries)/$maxTries) ... " -NoNewline

        $result = Invoke-RestMethod -Method Get -Uri $statusUrl -Headers @{"Authorization"="Bearer $accessToken"}
        $finished = $result.processingFinished;
        
        $tries++;
        if ($finished) {
            Write-Host "Finished" -ForegroundColor Green
        } else {
            Write-Host "In progress" -ForegroundColor DarkYellow
            Start-Sleep -Seconds 2
        }
    } while ($false -eq $finished -and $tries -le $maxTries)

    if (-not $finished) {
        Write-Host "Failed to get signed assemblies after $maxTries tries!" -ForegroundColor Red
    } else {
        Write-Host "";
        foreach ($assembly in $result.assemblies)
        {
            $color = [ConsoleColor]::Green;
            if ($assembly.fileProcessingState -eq "Error")
            {
                $color = [ConsoleColor]::Red;
                $success = $false;
            }
            Write-Host "------------------------------------------------------------------------------------------------------------------"
            Write-Host "FileName: $($assembly.fileName)" -ForegroundColor $color
            Write-Host "-> State: $($assembly.fileProcessingState)" -ForegroundColor $color
            $assembly.messages | Format-Table dateTime, messageType, message -Wrap| Out-String | Write-Host
        }
    }

    return $finished -and $success;
}

function DownloadSignedFiles($downloadUrl)
{
    $path = Join-Path $PWD "temp.zip"
    $expandPath = $destinationPath

    Write-Host "Save output to: $path ... " -NoNewline
    $_ = Invoke-WebRequest $downloadUrl -OutFile $path -Headers @{"Authorization"="Bearer $accessToken"}
    Write-Host "Saved output" -ForegroundColor Green

    Write-Host "Expanding archive to $expandPath, overriding files if needed ... " -NoNewline
    Expand-Archive -Path $path -DestinationPath $expandPath -Force
    Write-Host "Expanded archive" -ForegroundColor Green

    Write-Host "Removing temp archive: $path ... " -NoNewline
    Remove-Item -Path $path
    Write-Host "Removed" -ForegroundColor Green
}

function Delete($deleteUrl)
{
    Write-Host "Deleting uploaded package ... " -NoNewline
    $_ = Invoke-RestMethod $deleteUrl -Method Delete -Headers @{"Authorization"="Bearer $accessToken"}
    Write-Host "Deleted" -ForegroundColor Green
}

$links = UploadFiles($assemblies);
if ($null -ne $links)
{
    $statusUrl = $links.status
    $downloadUrl = $links.download
    $deleteUrl = $links.delete
    Write-Host "Status URL: $statusUrl"
    Write-Host "Download URL: $downloadUrl"
    Write-Host "Delete URL: $deleteUrl"

    $finished = CheckStatus -statusUrl $statusUrl
    if ($finished) {
        DownloadSignedFiles -downloadUrl $downloadUrl
        if ($deleteAfterwards) {
            Delete -deleteUrl $deleteUrl
        }
    } else {
        if ($deleteAfterwards) {
            Delete -deleteUrl $deleteUrl
        }
        exit 1;
    }
} else {
    exit 1;
}