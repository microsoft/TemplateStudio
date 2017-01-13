[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$versionNumber,
  [Parameter(Mandatory=$True,Position=2)]
  [string]$reposOwner,
  [Parameter(Mandatory=$True,Position=3)]
  [string]$reposName,
  [Parameter(Mandatory=$True,Position=4)]
  [string]$branch,
  [Parameter(Mandatory=$True,Position=5)]
  [string]$apiKey
)

Write-Host "Info..."
Write-Host "Version Number" $versionNumber
Write-Host "Repos Owner" $reposOwner
Write-Host "Repos Name" $reposName
Write-Host "Branch" $branch


$body = @{
  tag_name = $versionNumber;
  target_commitish= $branch;
  name= $versionNumber;
  body= "Release version " + $versionNumber;
}

$releaseParams = @{ 
   Uri = "https://api.github.com/repos/$reposOwner/$reposName/releases";     
   Method = 'POST'; 
   Headers = @{ 
     Authorization = 'Basic ' + [Convert]::ToBase64String( 
      [Text.Encoding]::ASCII.GetBytes($apiKey + ":x-oauth-basic")); 
    } 
    ContentType = 'application/json'; 
    Body = (ConvertTo-Json $body -Compress) 
  } 

 
Write-Host "Creating release in GitHub for version" $versionNumber

Invoke-RestMethod @releaseParams  

Write-Host 'Finished!'


