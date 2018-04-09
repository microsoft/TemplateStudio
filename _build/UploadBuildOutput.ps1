param
(
	[Parameter(Mandatory=$true)]
	[string]$storageAccountName,
  	[Parameter(Mandatory=$true)]
	[string]$containerName,
  	[Parameter(Mandatory=$true)]
	[string]$sourceLocation,
  	[Parameter(Mandatory=$true)]
	[string]$storageAccountKey,
  	[Parameter(Mandatory=$true)]
	[string]$versionNumber
)

$blobContext = New-AzureStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey

$files = Get-ChildItem $sourceLocation -rec | where {!$_.PSIsContainer}

foreach($file in $files)
{
	$fileName = $file.FullName
  	$blobName = "v$versionNumber/$file"
  	write-host "copying $fileName to $blobName"
  
  	Set-AzureStorageBlobContent -File $fileName -Container $containerName -Blob $blobName -Context $blobContext -Force
} 
