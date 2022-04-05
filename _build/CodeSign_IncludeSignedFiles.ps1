param
(
	[Parameter(Mandatory=$true)]
	[string]$vsixFilePath,
    [Parameter(Mandatory=$true)]
	[string]$inputPath
)

Add-Type -Assembly System.IO.Compression
Add-Type -Assembly System.IO.Compression.FileSystem

$files = Get-ChildItem -Recurse $inputPath | Where-Object { $_.FullName -like '*.dll' -or $_.FullName -like '*.js' }

$vsix = [System.IO.Compression.ZipFile]::Open($vsixFilePath, [System.IO.Compression.ZipArchiveMode]::Update)
$inputDir = (Get-Item "$inputPath\").Target

foreach ($file in $files)
{
    Write-Host "Processing file "$file.FullName
    $newFileName = $file.FullName.Replace("$inputDir\", '').Replace("\", "/")

    $entry = $vsix.Entries | Where-Object {$_.FullName -eq $newFileName}

    if ($entry)
    {
        Write-Host Deleted file $entry.FullName
        $entry.Delete()
    }
    else
    {
        Write-Error "$newFileName not in zip"
    }

    Write-Host Adding file $newFileName 
    Write-Host
    [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($vsix, $file.FullName, $newFileName) > $null 
}

$vsix.Dispose()
