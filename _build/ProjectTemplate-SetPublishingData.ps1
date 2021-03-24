[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$buildNumber,

  [Parameter(Mandatory=$True,Position=2)]
  [AllowEmptyString()]
  [string]$environmentname,

  [Parameter(Mandatory=$True, Position=3)]
  [AllowEmptyString()]
  [string]$environmentid
)

## EXPECTED VALUES: 
##  buildnumber in format pipelinename_0.22.21082.01
##  DEV: 
##    environmentname: "; dev-nightly"
##    environmentid:   ".dev"
##  PRE: 
##    environmentname: "; pre-release"
##    environmentid:   ".pre"
##  PRO: 
##    environmentname: ""
##    environmentid:   ""

$VersionRegex = "(\d+)\.(\d+)\.(\d+)\.(\d+)"

if($buildNumber -match $VersionRegEx){

  $versionNumber = [int]::Parse($matches[1]).ToString() + "." + [int]::Parse($matches[2]).ToString() + "." + [int]::Parse($matches[3]).ToString() + "." + [int]::Parse($matches[4]).ToString()
  Write-Host "Version Number" $versionNumber
  
}
else{
	throw "Build format does not match the expected pattern (buildName_w.x.y.z)"
}


## SET DATA IN PROJECT and ITEM TEMPLATE 
if(($environmentname -eq '' -and $environmentid -eq '') -or ($environmentname -like '; *' -and $environmentid -like '.*' )){
  Write-Host "Setting data in vs project and item template files"
 
  $languages = "\d*(de-DE).|(cs-CZ).|(es-ES).|(fr-FR).|(it-IT).|(ja-JP).|(ko-KR).|(pl-PL).|(pt-BR).|(ru-RU).|(tr-TR).|(zh-CN).|(zh-TW).\d*"


  $vstemplates = Get-ChildItem -include "*.vstemplate" -recurse |  Where-Object{ 
        $_.FullName -notmatch "\\Templates\\" -and 
        $_.FullName -notmatch "\\debug\\" -and
        $_.FullName -notmatch "\\obj\\" -and
        ($_.FullName -match "\\ProjectTemplates\\" -or $_.FullName -match "\\ItemTemplates\\")
    }



  if($vstemplates){
    foreach( $vstemplate in $vstemplates){
      if(Test-Path($vstemplate)){
        [xml]$templateContent = Get-Content $vstemplate
        
        Write-Host
        Write-Host $($vstemplate.Name)
        Write-Host "Orig values: $($templateContent.VSTemplate.TemplateData.Name), $($templateContent.VSTemplate.TemplateData.TemplateID), $versionNumber"        

        if ($templateContent.VSTemplate.TemplateData.Name -like "*; local*" -and $templateContent.VSTemplate.TemplateData.TemplateID -like "*.local")
        {
            if ($vstemplate -match $languages) {
                $rootTemplate = $vstemplate -replace $languages, ""
                if (($origRootTemplateFile -eq '') -or ($rootTemplate -ne $origRootTemplateFile)){
                    if(Test-Path($rootTemplate)){
                    
                        $origRootTemplateFile = $rootTemplate
                        [xml]$roottemplateContent = Get-Content $rootTemplate

                        if ($roottemplateContent.VSTemplate.TemplateData.Name -ne $templateContent.VSTemplate.TemplateData.Name -or $roottemplateContent.VSTemplate.TemplateData.TemplateID -ne $templateContent.VSTemplate.TemplateData.TemplateID)
                        {              
                            throw "Localized template name or id differs from root template name or id"
                        }
                    }
                    else
                    {
                        throw "Root template not found"
                    }
                }
            }

             ## Set version number
            $templateContent.VSTemplate.TemplateContent.CustomParameters.CustomParameter[2].Value = $versionNumber

            #Replace template name
            $templateContent.VSTemplate.TemplateData.Name = $templateContent.VSTemplate.TemplateData.Name -replace "; local", "$environmentname"

            #Replace template id
            $templateContent.VSTemplate.TemplateData.TemplateID = $templateContent.VSTemplate.TemplateData.TemplateID -replace ".local", $environmentid

            Write-Host "New values:  $($templateContent.VSTemplate.TemplateData.Name), $($templateContent.VSTemplate.TemplateData.TemplateID), $versionNumber"                   

            $templateContent.Save($vstemplate) 
           
        }
        else
        {
            throw "Template Name must include '; local' and Template ID must include '.local'"
        }

      }
    }
  }
  else{
    throw "No Project Template files found."
  }
}
else{
  throw "Invalid environmentname or environmentid"
}
