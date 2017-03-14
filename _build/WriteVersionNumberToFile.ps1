[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$buildNumber,
  [Parameter(Mandatory=$True,Position=2)]
  [string]$destinationDirectory,
  [Parameter(Mandatory=$True,Position=3)]
  [string]$destinationFile
)

Write-Host "Info..."
Write-Host "Version Number" $versionNumber
Write-Host "Destination Directory" $destinationDirectory
Write-Host "Destination File" $destinationFile

$TfsBuildVersionRegEx = "_(\d+)\.(\S+)\.(\d+)"

if($buildNumber -match $TfsBuildVersionRegEx){

	$versionNumber = $matches[0].Replace("_","")
    Write-Host "Version Number" $versionNumber
}
else{
	throw "Build format does not match the expected pattern (buildName_version)"
}

Write-Host "Writing" $versionNumber "to" $destinationPath

New-Item $destinationDirectory\$destinationFile -type file -force -value $versionNumber


