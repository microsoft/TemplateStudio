[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$sourcePath,
  [Parameter(Mandatory=$True,Position=2)]
  [string]$destinationDirectory,
  [Parameter(Mandatory=$True,Position=3)]
  [string]$destinationFileName
)

Write-Host "Info..."
Write-Host "Source Path" $sourcePath
Write-Host "Destination Directory" $destinationDirectory
Write-Host "Destination File Name" $destinationFileName


if (!(Test-Path -path $destinationDirectory)) 
{
    Write-Host "Creating destination directory" $destinationDirectory
    New-Item $destinationDirectory -Type Directory
}


$destinationPath = Join-Path $destinationDirectory $destinationFileName

Write-Host "Copying" $sourcePath "to" $destinationPath

Copy-Item -Path $sourcePath -Destination $destinationPath  

Write-Host 'Finished!'