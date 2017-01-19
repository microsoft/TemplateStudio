[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$tfsBuildNumber,

  [Parameter(Mandatory=$True,Position=2)]
  [string]$vsixIdentity,

  [Parameter(Mandatory=$False,Position=3)]
  [string]$includeFilePathPattern = "code\\src\\"
)

$VersionRegex = "\d+\.\d+\.\d+\.\d+"

if($buildNumber -match $VersionRegEx){
	$versionNumber = $matches[0]
  Write-Host "Version Number" $versionNumber
}
else{
	throw "Build format does not match the expected pattern (buildName_w.x.y.z)"
}

if($vsixIdentity){
  Write-Host "Setting Identity in VSIX manifest"
  $vsixManifestFile = Get-ChildItem -include "*source.extension.vsixmanifest" -recurse
  [xml]$manifestContent = Get-Content $vsixManifestFile

  $manifestContent.PackageManifest.Metadata.Identity.Id = $vsixIdentity
  $manifestContent.PackageManifest.Metadata.Identity.Version = $versionNumber
  $manifestContent.Save($vsixManifest) 
  Write-Host "$vsixManifestFile.FullName - Version & Identity applied ($versionNumber, $vsixIdentity)"
}
else{
  throw "Identity is mandatory."
}

Write-Host "Applying version to AssemblyInfo Files in matching the path pattern '$includeFilePathPattern'" 
$files = Get-ChildItem -include "*AssemblyInfo.cs" -Recurse | Where-Object{ $_.FullName -match $includeFilePathPattern}
if($files)
{
    Write-Host "Will apply $versionNumber to $($files.count) files."

    foreach ($file in $files) {
        $filecontent = Get-Content($file)
        attrib $file -r
        $filecontent -replace $VersionRegex, $versionNumber | Out-File $file utf8
        Write-Host "$file.FullName - version applied"
    }
}
else
{
    Write-Warning "No files found to apply version."
}
