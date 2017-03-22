[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$vsixManifestFile,

  [Parameter(Mandatory=$True,Position=2)]
  [string]$vsixIdentity,

  [Parameter(Mandatory=$True,Position=3)]
  [string]$vsixDisplayName,

  [Parameter(Mandatory=$True,Position=4)]
  [string]$buildNumber,

  [Parameter(Mandatory=$False,Position=3)]
  [string]$publicKeyToken = "e4ef4cc7a47ae0c5" #TestKey.snk
)

$VersionRegex = "(\d+)\.(\d+)\.(\d+)\.(\d+)"

if($buildNumber -match $VersionRegEx){

  Write-Output "Parsed Date From Build: $dateFromBuildNumber"

  $revision =  [int]::Parse($matches[4]).ToString()

  if($buildNumber.ToLower().StartsWith("dev")){
    $revision = (65000 + [int]::Parse($matches[4])).ToString();
  }
  
  $versionNumber = [int]::Parse($matches[1]).ToString() + "." + [int]::Parse($matches[2]).ToString() + "." + [int]::Parse($matches[3]).ToString() + "." + $revision
  Write-Host "Version Number" $versionNumber
  
}
else{
	throw "Build format does not match the expected pattern (buildName_w.x.y.z)"
}

## SET IDENTITY AND VERSION IN VSIX Manifest
if($vsixIdentity){
  Write-Host "Setting Identity in VSIX manifest"
  if(Test-Path($vsixManifestFile)){
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

## APPLY VERSION TO ASSEMBLY FILES (AssemblyVersion and AssemblyFileVersion)
Write-Host "Applying version to AssemblyInfo Files in matching the path pattern '$codePathPattern'" 
$files = Get-ChildItem -include "*AssemblyInfo.cs" -Recurse |  Where-Object{ $_.FullName -notmatch "\\Templates\\" }
if($files)
{
    Write-Host "Will apply $versionNumber to $($files.count) files."

    $assemblyVersionRegEx = "\(""$VersionRegex""\)" 
    $assemblyVersionReplacement = "(""$versionNumber"")"

    foreach ($file in $files) {
        $filecontent = Get-Content($file)
        attrib $file -r
        $filecontent -replace $assemblyVersionRegEx, $assemblyVersionReplacement | Out-File $file utf8
        Write-Host "$file - version applied"
    }
}
else
{
    Write-Warning "No files found to apply version."
}

## APPLY VERSION TO PROJECT TEMPLATE WIZARD REFERENCE
if($publicKeyToken){
  Write-Host "Setting Wizard Extension configuration in Project Template"
  $projectTemplate = Get-ChildItem -include "*.vstemplate" -recurse |  Where-Object{ 
        $_.FullName -notmatch "\\Templates\\" -and 
        $_.FullName -notmatch "\\debug\\" -and
        $_.FullName -notmatch "\\obj\\" -and
        $_.FullName -match "\\ProjectTemplates\\"
    }
  if($projectTemplate){
  

    [xml]$projectTemplateContent = Get-Content $projectTemplate

    $newPublicKeyToken = "PublicKeyToken=$publicKeyToken"
    $wizardAssemblyStrongName = $projectTemplateContent.VSTemplate.WizardExtension.Assembly -replace $VersionRegEx, $versionNumber 
    $wizardAssemblyStrongName = $wizardAssemblyStrongName -replace "PublicKeyToken=.*</Assembly>", "$newPublicKeyToken</Assembly>"

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
}