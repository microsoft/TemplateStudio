[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$uwpname,

  [Parameter(Mandatory=$True,Position=2)]
  [string]$wpfname,

  [Parameter(Mandatory=$True,Position=3)]
  [string]$winuidesktopname,

  [Parameter(Mandatory=$True,Position=4)]
  [string]$winuiuwpname,

  [Parameter(Mandatory=$True,Position=5)]
  [string]$csharpuwptemplateId,

  [Parameter(Mandatory=$True,Position=6)]
  [string]$csharpwpftemplateId,

  [Parameter(Mandatory=$True,Position=7)]
  [string]$csharpwinuitemplateId,

  [Parameter(Mandatory=$True,Position=8)]
  [string]$cppwinudesktopitemplateId,

  [Parameter(Mandatory=$True,Position=9)]
  [string]$cppwinuuwpitemplateId,

  [Parameter(Mandatory=$True,Position=10)]
  [string]$visualbasictemplateId,

  [Parameter(Mandatory=$True,Position=11)]
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

## SET DATA IN PROJECT TEMPLATE 
if($uwpname -and $wpfname){
  Write-Host "Setting data in vs project template files"

  $projectTemplates = Get-ChildItem -include "*.vstemplate" -recurse |  Where-Object{ 
        $_.FullName -notmatch "\\Templates\\" -and 
        $_.FullName -notmatch "\\debug\\" -and
        $_.FullName -notmatch "\\obj\\" -and
        $_.FullName -match "\\ProjectTemplates\\"
    }

  if($projectTemplates){
    foreach( $projectTemplate in $projectTemplates){
      if(Test-Path($projectTemplate)){
        [xml]$templateContent = Get-Content $projectTemplate

        $templateContent.VSTemplate.TemplateContent.CustomParameters.CustomParameter[2].Value = $versionNumber
        if($templateContent.VSTemplate.TemplateData.TemplateID -eq 'Microsoft.CSharp.UWP.WindowsTemplateStudio.local')
        {
           $templateContent.VSTemplate.TemplateData.TemplateID = $csharpuwptemplateId
           $templateContent.VSTemplate.TemplateData.Name = $uwpname

           Write-Host "$projectTemplate - Name, TemplateId & Version applied ($uwpname, $csharpuwptemplateId, $versionNumber)"

        }

        if($templateContent.VSTemplate.TemplateData.TemplateID -eq 'Microsoft.CSharp.WPF.WindowsTemplateStudio.local')
        {
           $templateContent.VSTemplate.TemplateData.TemplateID = $csharpwpftemplateId
           $templateContent.VSTemplate.TemplateData.Name = $wpfname

           Write-Host "$projectTemplate - Name, TemplateId & Version applied ($wpfname, $csharpwpftemplateId, $versionNumber)"
        }

       if($templateContent.VSTemplate.TemplateData.TemplateID -eq 'Microsoft.CSharp.WinUI.WindowsTemplateStudio.local')
        {
           $templateContent.VSTemplate.TemplateData.TemplateID = $csharpwinuitemplateId
           $templateContent.VSTemplate.TemplateData.Name = $winuidesktopname

           Write-Host "$projectTemplate - Name, TemplateId & Version applied ($winuidesktopname, $csharpwinuitemplateId, $versionNumber)"
        }

        if($templateContent.VSTemplate.TemplateData.TemplateID -eq 'Microsoft.Cpp.WinUI.Desktop.WindowsTemplateStudio.local')
        {
           $templateContent.VSTemplate.TemplateData.TemplateID = $cppwinudesktopitemplateId
           $templateContent.VSTemplate.TemplateData.Name = $winuidesktopname

           Write-Host "$projectTemplate - Name, TemplateId & Version applied ($winuidesktopname, $cppwinudesktopitemplateId, $versionNumber)"
        }

        if($templateContent.VSTemplate.TemplateData.TemplateID -eq 'Microsoft.Cpp.WinUI.UWP.WindowsTemplateStudio.local')
        {
           $templateContent.VSTemplate.TemplateData.TemplateID = $cppwinuuwpitemplateId
           $templateContent.VSTemplate.TemplateData.Name = $winuiuwpname

           Write-Host "$projectTemplate - Name, TemplateId & Version applied ($winuiuwpname, $cppwinuuwpitemplateId, $versionNumber)"
        }

        if($templateContent.VSTemplate.TemplateData.TemplateID -eq 'Microsoft.VisualBasic.UWP.WindowsTemplateStudio.local')
        {
           $templateContent.VSTemplate.TemplateData.TemplateID = $visualbasictemplateId
           $templateContent.VSTemplate.TemplateData.Name = $uwpname

           Write-Host "$projectTemplate - Name, TemplateId & Version applied ($uwpname, $visualbasictemplateId, $versionNumber)"

        }

        $templateContent.Save($projectTemplate) 

      }
    }
  }
  else{
    throw "No Project Template files found."
  }
}
else{
  throw "Name is mandatory."
}