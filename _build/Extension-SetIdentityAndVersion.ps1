[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$vsixManifestFile,

  [Parameter(Mandatory=$True,Position=2)]
  [string]$vsixIdentity,

  [Parameter(Mandatory=$True,Position=3)]
  [string]$vsixDisplayName,

  [Parameter(Mandatory=$True,Position=4)]
  [string]$buildNumber
)

$VersionRegex = "(\d+)\.(\d+)\.(\d+)\.(\d+)"

if($buildNumber -match $VersionRegEx){

  $versionNumber = [int]::Parse($matches[1]).ToString() + "." + [int]::Parse($matches[2]).ToString() + "." + [int]::Parse($matches[3]).ToString() + "." + [int]::Parse($matches[4]).ToString()
  Write-Host "Version Number" $versionNumber
  
}
else{
	throw "Build format does not match the expected pattern (buildName_w.x.y.z)"
}

## SET IDENTITY AND VERSION IN VSIX Manifest
if(Test-Path($vsixIdentity)){
  Write-Host "Setting Identity in VSIX manifest"
  if($vsixManifestFile){
    [xml]$manifestContent = Get-Content $vsixManifestFile
    $manifestContent.PackageManifest.Metadata.Identity.Id = $vsixIdentity
    $manifestContent.PackageManifest.Metadata.Identity.Version = $versionNumber
    $manifestContent.PackageManifest.Metadata.DisplayName = $vsixDisplayName
    $manifestContent.Save($vsixManifestFile) 
    Write-Host "$vsixManifestFile - Version, Identity & DisplayName applied ($versionNumber, $vsixIdentity, $vsixDisplayName)"
  }
  else{
    throw "No VSIX manifest file found."
  }
}
else{
  throw "Identity is mandatory."
}

## APPLY VERSION TO ASSEMBLY FILES
Write-Host "Applying version to AssemblyInfo Files in matching the path pattern '$codePathPattern'" 
$files = Get-ChildItem -include "*AssemblyInfo.cs" -Recurse |  Where-Object{ $_.FullName -notmatch "\\Templates\\" }
if($files)
{
    Write-Host "Will apply $versionNumber to $($files.count) files."

    $assemblyFileVersionRegEx = "AssemblyFileVersion\(""(\d+)\.(\d+)\.(\d+)\.(\d+)""\)" 
    $assemblyFileVersionReplacement = "AssemblyFileVersion(""$versionNumber"")"

    foreach ($file in $files) {
        $filecontent = Get-Content($file)
        attrib $file -r
        $filecontent -replace $assemblyFileVersionRegEx, $assemblyFileVersionReplacement | Out-File $file utf8
        Write-Host "$file - version applied"
    }
}
else
{
    Write-Warning "No files found to apply version."
}