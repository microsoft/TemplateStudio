[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$versionNumber,
  [Parameter(Mandatory=$True,Position=2)]
  [string]$destinationDirectory,
  [Parameter(Mandatory=$True,Position=3)]
  [string]$destinationFile
)
Write-Host "Info..."
Write-Host "Version Number" $versionNumber
Write-Host "Destination Directory" $destinationDirectory
Write-Host "Destination File" $destinationFile

Write-Host "Writing" $versionNumber "to" $destinationPath

New-Item $destinationDirectory\$destinationFile -type file -force -value $versionNumber


