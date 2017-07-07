[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$vsTemplatesPath,

  [Parameter(Mandatory=$True,Position=2)]
  [string]$name,

  [Parameter(Mandatory=$True,Position=3)]
  [string]$templateId,

  [Parameter(Mandatory=$True,Position=4)]
  [string]$buildNumber,

  [Parameter(Mandatory=$false,Position=5)]
  [string]$description
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
if($name){
  Write-Host "Setting data in vs project template files"

  $projectTemplates = Get-ChildItem -include "$vsTemplatesPath\*.vstemplate" -recurse |  Where-Object{ 
        $_.FullName -notmatch "\\Templates\\" -and 
        $_.FullName -notmatch "\\debug\\" -and
        $_.FullName -notmatch "\\obj\\" -and
        $_.FullName -match "\\ProjectTemplates\\"
    }

  if($projectTemplates){
    foreach( $projectTemplate in $projectTemplates){
      if(Test-Path($projectTemplate)){
        [xml]$templateContent = Get-Content $projectTemplate
        $templateContent.VSTemplate.TemplateData.Name = $name
        $templateContent.VSTemplate.TemplateData.TemplateID = $templateId
        $templateContent.VSTemplate.TemplateContent.CustomParameters.CustomParameter[1].Value = $versionNumber
      
        if($description)
        {
          $templateContent.VSTemplate.TemplateData.Description = $description
        }

        $templateContent.Save($projectTemplate) 
        Write-Host "$projectTemplate - Name, TemplateId, Version & Description applied ($name, $templateId, $versionNumber, $description)"
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