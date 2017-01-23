[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$buildNumber,

  [Parameter(Mandatory=$True,Position=2)]
  [string]$vsixIdentity,

  [Parameter(Mandatory=$False,Position=3)]
  [string]$publicKeyToken = "e4ef4cc7a47ae0c5" #TestKey.snk
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
if($vsixIdentity){
  Write-Host "Setting Identity in VSIX manifest"
  $vsixManifestFile = Get-ChildItem -include "*source.extension.vsixmanifest" -recurse | Where-Object{ $_.FullName -notmatch "\\Templates\\" }
  if($vsixManifestFile){
    [xml]$manifestContent = Get-Content $vsixManifestFile
    $manifestContent.PackageManifest.Metadata.Identity.Id = $vsixIdentity
    $manifestContent.PackageManifest.Metadata.Identity.Version = $versionNumber
    $manifestContent.Save($vsixManifestFile) 
    Write-Host "$vsixManifestFile - Version & Identity applied ($versionNumber, $vsixIdentity)"
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

## APPLY VERSION TO PROJECT TEMPLATE WIZARD 
#Commented until review behavior of TemplatesEngine verified with strong name version
<#if($publicKeyToken){
  Write-Host "Setting Wizard Extension configuration in Project Template"
  $projectTemplate = Get-ChildItem -include "*.vstemplate" -recurse |  Where-Object{ $_.FullName -notmatch "\\Templates\\" -and $_.FullName -match "\\vspt\\"}
  if($projectTemplate){
    [xml]$projectTemplateContent = Get-Content $projectTemplate

    $wizardAssemblyStrongName = $projectTemplateContent.VSTemplate.WizardExtension.Assembly
    $wizardAssemblyStrongName = $wizardAssemblyStrongName -replace $VersionRegEx, $versionNumber 
    $wizardAssemblyStrongName = $wizardAssemblyStrongName -replace "PublicKeyToken=.*", "PublicKeyToken=$publicKeyToken"

    $projectTemplateContent.VSTemplate.WizardExtension.Assembly = $wizardAssemblyStrongName
    
    $projectTemplateContent.Save($projectTemplate)

    Write-Host "$projectTemplate - Wizard Assembly Strong Name updated ($wizardAssemblyStrongName)"
  }
  else{
    throw "No Project Template manifest file found!"
  }
}
else{
  throw "Public key token not set."
}#>