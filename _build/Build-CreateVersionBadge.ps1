[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$Identifier,
  [Parameter(Mandatory=$False,Position=2)]
  [string]$Version,
  [Parameter(Mandatory=$False,Position=3)]
  [string]$Color = "blue"
)
$ext = "svg"
$file = ".\img.$Identifier.version.$ext"

function GetVersionNumberFromBuildNumber
{
  Param(
    [string]$buildNumber
  )
  $VersionNumber = "Undefined_Version"
  $VersionRegex = "(\d+)\.(\d+)\.(\d+)\.(\d+)"
  if($buildNumber -match $VersionRegEx){
    $revision =  [int]::Parse($matches[4]).ToString()
  
    if($buildNumber.ToLower().StartsWith("dev")){
      $revision = (65000 + [int]::Parse($matches[4])).ToString();
    }
    
    $VersionNumber = [int]::Parse($matches[1]).ToString() + "." + [int]::Parse($matches[2]).ToString() + "." + [int]::Parse($matches[3]).ToString() + "." + $revision
  }

  return $VersionNumber
}

Write-Host "Getting the badge..."

if(!$Version){
  $Version = GetVersionNumberFromBuildNumber $Env:BUILD_BUILDNUMBER
  $Version = "v$Version"
}

Remove-Item $file -Force -ErrorAction SilentlyContinue
Invoke-WebRequest -Uri "https://img.shields.io/badge/$Identifier-$Version-$Color.$ext" -OutFile $file

$keys = Get-AzureRmStorageAccountKey -StorageAccountName "wtsrepository" -ResourceGroupName "WTS"
$context = New-AzureStorageContext -StorageAccountName "wtsrepository" -StorageAccountKey $keys[0].value

Write-Host "Uploading badge to wtsrepository badges container..."
$properties = @{"ContentType"="image/svg+xml"; "CacheControl"="no-cache"}
Set-AzureStorageBlobContent -Container "badges" -File $file -Properties $properties -Context $context -Force

