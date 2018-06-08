# THIS SCRITP WILL NEED TO BE SPLITED NEXT TIME, GETTING TO LONG.
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

  [Parameter(Mandatory=$True,Position=5)]
  [string]$vsixProjFile,

  [Parameter(Mandatory=$True,Position=6)]
  [string]$isProductComponent,

  [Parameter(Mandatory=$True,Position=7)]
  [string]$vsixCommandMenuName,

  [Parameter(Mandatory=$True,Position=8)]
  [string]$packageGuid,

  [Parameter(Mandatory=$True,Position=9)]
  [string]$cmdSetGuid,

  [Parameter(Mandatory=$False,Position=10)]
  [string]$targetVsixCommandMenuName = "Windows Template Studio (local)",

  [Parameter(Mandatory=$False,Position=11)]
  [string]$targetPackageGuid = "ae1b4c32-9c93-45b8-a36b-8734f4b120dd",

  [Parameter(Mandatory=$False,Position=12)]
  [string]$targetCmdSetGuid = "caa4fb82-0dca-40fe-bae0-081e0f96226f",
  
  [Parameter(Mandatory=$False,Position=13)]
  [string]$publicKeyToken = "e4ef4cc7a47ae0c5" #TestKey.snk
)

$VersionRegex = "(\d+)\.(\d+)\.(\d+)\.(\d+)"

if($buildNumber -match $VersionRegEx){

  Write-Output "Parsed Date From Build: $dateFromBuildNumber"

  $revision =  [int]::Parse($matches[4]).ToString()
  
  $versionNumber = [int]::Parse($matches[1]).ToString() + "." + [int]::Parse($matches[2]).ToString() + "." + [int]::Parse($matches[3]).ToString() + "." + $revision
  Write-Host "Version Number" $versionNumber
  
}
else{
	throw "Build format does not match the expected pattern (buildName_w.x.y.z)"
}

## SET IDENTITY AND VERSION IN VSIX Manifest
if($vsixIdentity){
  Write-Host
  Write-Host "Setting Identity in VSIX manifest"
  if(Test-Path($vsixManifestFile)){
    [xml]$manifestContent = Get-Content $vsixManifestFile
    $manifestContent.PackageManifest.Metadata.Identity.Id = $vsixIdentity
    $manifestContent.PackageManifest.Metadata.Identity.Version = $versionNumber
    $manifestContent.PackageManifest.Metadata.DisplayName = $vsixDisplayName
    $resolvedPath = Resolve-Path($vsixManifestFile)
    $manifestContent.Save($resolvedPath) 
    Write-Host "$resolvedPath - Version, Identity & DisplayName applied ($versionNumber, $vsixIdentity, $vsixDisplayName)"

    $vsixLangPacks = Get-ChildItem -include "Extension.vsixlangpack" -recurse |  Where-Object{ 
        $_.FullName -notmatch "\\Templates\\" -and 
        $_.FullName -notmatch "\\debug\\" -and
        $_.FullName -notmatch "\\obj\\" -and
        $_.FullName -match "\\Installer.2017\\"
    }
    if($vsixLangPacks){ 
      Write-Host "Applying Display Name to vsixlangpack files"
      foreach ($langPack in $vsixLangPacks) {
        [xml]$langContent = Get-Content $langPack
        $langContent.VsixLanguagePack.LocalizedName = $vsixDisplayName
        $langContent.Save($langPack) 
        Write-Host "$langPack - LocalizedName applied ($vsixDisplayName)"        
      }
    }
  }
  else{
    throw "No VSIX manifest file found."
  }
}
else{
  throw "Identity is mandatory."
}


## SET ISPRODUCTCOMPONENT IN VSIX PROJECT FILE
if($isProductComponent){
  Write-Host
  Write-Host "Setting IsProductComponent in VSIX project file"
  if(Test-Path($vsixProjFile)){
    [xml]$projFileContent = Get-Content $vsixProjFile

    $isProductComponentPropertyGroup = $projFileContent.Project.PropertyGroup | ? {$_.IsProductComponent}
    $isProductComponentPropertyGroup.IsProductComponent = $isProductComponent
   
    $resolvedProjFilePath = Resolve-Path($vsixProjFile)
    $projFileContent.Save($resolvedProjFilePath) 
    Write-Host "$resolvedProjFilePath - IsProductComponent ($isProductComponent)"

  }
  else{
    throw "No VSIX project file found."
  }
}
else{
  throw "IsProductComponent is mandatory."
}

## REPLACE Command Guids
if($cmdSetGuid -and $packageGuid){
  Write-Host
  Write-Host "Setting PackageGuid and CmdSetGuid in VSCT Files"
  $vsctFiles = Get-ChildItem -include "*.vsct" -recurse |  Where-Object{ 
      $_.FullName -notmatch "\\Templates\\" -and 
      $_.FullName -notmatch "\\debug\\" -and
      $_.FullName -notmatch "\\obj\\" -and
      $_.FullName -match "\\Installer.2017\\Commands"
  }
  if($vsctFiles){ 
    Write-Host
    Write-Host "Applying guids $cmdSetGuid, $packageGuid and command name $vsixCommandMenuName to VSCT Files"
    foreach ($vsctFile in $vsctFiles) {
      $vsctFileContent = Get-Content $vsctFile
      attrib $vsctFile -r
      $replacedVsctContent = $vsctFileContent.Replace($targetPackageGuid, $packageGuid)
      $replacedVsctContent = $replacedVsctContent.Replace($targetCmdSetGuid, $cmdSetGuid)
      $replacedVsctContent = $replacedVsctContent.Replace($targetVsixCommandMenuName, $vsixCommandMenuName)
      $replacedVsctContent | Out-File $vsctFile -Encoding utf8 -Force
      Write-Host "$vsctFile - Guids applied (PackageGuid:$packageGuid, CmdGuid:$cmdSetGuid)"        
    }
  }
  else{
    throw "No VSCT files found."
  }

  Write-Host
  Write-Host "Applying guids $cmdSetGuid, $packageGuid to PackageIds.cs"
  $path = Resolve-Path $vsixManifestFile
  $installerPath = Split-Path $path
  $constFile = Join-Path  $installerPath "Commands\PackageIds.cs"
  
  if($constFile){
    $constFileContent = Get-Content $constFile
    attrib $constFile -r
    $replacedConstContent = $constFileContent.Replace($targetPackageGuid, $packageGuid).Replace($targetCmdSetGuid, $cmdSetGuid)
    $replacedConstContent | Out-File $constFile -Encoding utf8 -Force
    Write-Host "$constFile - Guids applied (PackageGuid:$packageGuid, CmdGuid:$cmdSetGuid)"
  }
  else{
     throw "PackageIds.cs constants file not found"
  }
}
else{
  throw "PackageGuid and CmdSetGuid are mandatory"
}


## APPLY VERSION TO ASSEMBLY FILES (AssemblyVersion and AssemblyFileVersion)
Write-Host
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


## APPLY VERSION RelayCommandPackage
Write-Host
Write-Host "Applying version to RelayCommandPackage.cs"
$files = Get-ChildItem -include "RelayCommandPackage.cs" -Recurse |  Where-Object{ $_.FullName -notmatch "\\Templates\\" }
if($files)
{
    Write-Host "Will apply $versionNumber to $($files.count) files."

    foreach ($file in $files) {
        $filecontent = Get-Content($file)
        attrib $file -r
        $filecontent -replace $VersionRegex, $versionNumber | Out-File $file utf8
        Write-Host "$file - version applied"
    }
}
else
{
    Write-Warning "No relaycommandPackage found to apply version."
}

## APPLY VERSION TO PROJECT TEMPLATE WIZARD REFERENCE
if($publicKeyToken){
  Write-Host
  Write-Host "Setting Wizard Extension configuration in Project Template"
  $projectTemplates = Get-ChildItem -include "*.vstemplate" -recurse |  Where-Object{ 
        $_.FullName -notmatch "\\Templates\\" -and 
        $_.FullName -notmatch "\\debug\\" -and
        $_.FullName -notmatch "\\obj\\" -and
        $_.FullName -match "\\ProjectTemplates\\"
    }
  if($projectTemplates){
  
    foreach( $projectTemplate in $projectTemplates){
      [xml]$projectTemplateContent = Get-Content $projectTemplate

      $wizardAssemblyStrongName = $projectTemplateContent.VSTemplate.WizardExtension.Assembly -replace $VersionRegEx, $versionNumber 
      $wizardAssemblyStrongName = $wizardAssemblyStrongName -replace "PublicKeyToken=.*", "PublicKeyToken=$publicKeyToken"

      $projectTemplateContent.VSTemplate.WizardExtension.Assembly = $wizardAssemblyStrongName
      
      $projectTemplateContent.Save($projectTemplate)

      Write-Host "$projectTemplate - Wizard Assembly Strong Name updated ($wizardAssemblyStrongName)"
    }
  }
  else{
    throw "No Project Template manifest file found!"
  }
}
else{
  throw "Public key token not set."
}
