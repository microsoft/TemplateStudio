param
(
	[Parameter(Mandatory=$true)]
	[string]$vsixFilePath,
    [Parameter(Mandatory=$true)]
	[string]$outputPath
)

Add-Type -Assembly System.IO.Compression.FileSystem

$vsix = [IO.Compression.ZipFile]::OpenRead($vsixFilePath)
Write-Output "Listing entries"
$entries=$vsix.Entries | Where-Object { $_.FullName -like '*.dll' -or $_.FullName -like '*.js' -and ($_.FullName.StartsWith('TemplateStudio') -or -not $_.FullName.StartsWith('Microsoft')) -and -not $_.FullName.StartsWith('System') }

Write-Output $entries.FullName 

New-Item -ItemType Directory -Path $outputPath -Force

foreach ($entry in $entries)
{
    $entryTargetFilePath = [System.IO.Path]::Combine($outputPath, $entry.FullName)
    $entryDir = [System.IO.Path]::GetDirectoryName($entryTargetFilePath)

    if(!(Test-Path $entryDir)) {
        New-Item -ItemType Directory -Path $entryDir | Out-Null 
    }

    [System.IO.Compression.ZipFileExtensions]::ExtractToFile($entry, $entryTargetFilePath, $true);
    Write-Host "<file src=""__INPATHROOT__\$entryTargetFilePath"" signType=""400"" dest=""__OUTPATHROOT__\$entryTargetFilePath"" />"
}

$vsix.Dispose()
