[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$buildNumber,

  [Parameter(Mandatory=$True,Position=2)]
  [string]$vsixIdentity,

  [Parameter(Mandatory=$False,Position=3)]
  [string]$publicKeyToken = "e4ef4cc7a47ae0c5" #TestKey.snk
)

$VersionRegex = "\d+\.\d+\.\d+\.\d+"

if($buildNumber -match $VersionRegEx){
	$versionNumber = $matches[0]
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
    $manifestContent.Save($vsixManifest) 
    Write-Host "$vsixManifestFile.FullName - Version & Identity applied ($versionNumber, $vsixIdentity)"
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

## APPLY VERSION TO PROJECT TEMPLATE WIZARD REFERENCE
if($publicKeyToken){
  Write-Host "Setting Wizard Extension configuration in Project Template"
  $projectTemplate = Get-ChildItem -include "*-vstemplate" -recurse |  Where-Object{ $_.FullName -notmatch "\\Templates\\" }
  if($projectTemplate){
    [xml]$projectTemplateContent = Get-Content $projectTemplate

    $newPublicKeyToken = "PublicKeyToken=$publicKeyToken"

    $wizardAssemblyStrongName = $projectTemplateContent.VSTemplate.WizardExtension.Assembly -replace $VersionRegEx, $versionNumber 
    $wizardAssemblyStrongName = $wizardExtensioAssembly -replace "PublicKeyToken=.*</Assembly>", "$newPublicKeyToken</Assembly>"

    $projectTemplateContent.VSTemplate.WizardExtension.Assembly = $wizardAssemblyStrongName
    
    Write-Host "$projectTemplate.FullName - Wizard Assembly Strong Name updated ($wizardAssemblyStrongName)"
  }
  else{
    throw "No Project Template manifest file found!"
  }
}
else{
  throw "Public key token not set."
}