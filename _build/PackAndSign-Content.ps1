[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$source,
  [Parameter(Mandatory=$True,Position=2)]
  [string]$destinationDirectory,
  [Parameter(Mandatory=$True,Position=3)]
  [string]$signingCertThumbprint,
  [Parameter(Mandatory=$true,Position=4)]
  [string]$coreAssemblyPath
)

$command = "Command Excuted: " + $MyInvocation.Line
Write-Output $command

Write-Host "Source: " $source
Write-Host "Destination Directory: " $destinationDirectory
Write-Host "Signing Cert Thumbprint: " $signingCertThumbprint
Write-Host "Core Assembly Path: " $coreAssemblyPath

$addType = Add-Type -Path $coreAssemblyPath -PassThru -ErrorAction Stop
if($addType){
  $resultPack = [Microsoft.Templates.Core.Locations.Templatex]::PackAndSign($source, $signingCertThumbprint, "text/plain")
  
  if($resultPack){
    if (!(Test-Path -path $destinationDirectory)) 
    {
        Write-Host "Creating destination directory" $destinationDirectory
        New-Item $destinationDirectory -Type Directory -Force
    }
    $destinationPath = Join-Path $destinationDirectory $destinationFileName

    Write-Host "Copying" $resultPack "to" $destinationPath
    Copy-Item -Path $resultPack -Destination $destinationPath -Force
  }
  else{
    throw "Source not packed and signed properly!"
  }
}
else{
  throw "Core Assembly not found. Can't continue.!"
}
Write-Host 'Finished!'