<#
.SYNOPSIS
Import localized files from TDBuild.
    
.PARAMETER Path
The path to the localized TDBuild output folder downloaded from the localization pipeline.

.EXAMPLE
Import-TDBuild.ps1

.EXAMPLE
Import-TDBuild.ps1 ~\Downloads\TDBuild
#>

[CmdletBinding()]
Param(
    [string]$Path = "~\Downloads\TDBuild"
)

Get-ChildItem $Path -File -Recurse |
Where-Object { -not $_.Directory.Name.StartsWith("qps-ploc") } |
ForEach-Object {
    $repositoryRoot = git rev-parse --show-toplevel

    $targetPath = Join-Path $repositoryRoot $_.Directory.Parent.FullName.Substring($_.FullName.LastIndexOf("\code\"))
    $targetFileName = "{0}.{1}{2}" -f $_.BaseName, $_.Directory.Name, $_.Extension

    Copy-Item $_ (Join-Path $targetPath $targetFileName) -Verbose
}