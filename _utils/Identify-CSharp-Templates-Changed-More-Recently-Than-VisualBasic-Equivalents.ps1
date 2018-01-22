## This script will tell us all CS template files that have been modified more recently than their VB equivalents
## May include false positives (CS file changed but VB one doesn't need changing)
## May also include false negatives (VB file changed but not to be semantically equivalent to the CS one)

## This script cannot be perfect in identifying everything. It should be used in combination with the script that looks at Git Commits.

function Get-CsEquivalentFile($vbfile)
{
    return $vbfile -replace "VB\\", "\" `
                   -replace ".vb", ".cs"`
                   -replace "My Project", "Properties"
}

# Get list of all templates
$allTemplates = Get-ChildItem ..\templates\* -Recurse -include template.json | where { $_.FullName -notmatch "\\templates\\test\\" } | % { Write-Output $_.FullName }
Foreach ($t in $allTemplates)
{
    # find VB ones
    if ($t -match "VB\\")
    {
        $templateName = $t -replace "\\.template.config\\template.json", ""

        Get-ChildItem $templateName -Recurse -include *.* -Attributes !Directory | % { 

            $vbfile = $_.FullName
            $vbtimestamp = $_.LastWriteTimeUtc

            $csequiv = Get-CsEquivalentFile $_

            # If CS equivalent of VB file has been removed then need to understand why/how removed and make similar changes for VB version
            if (-not (Test-Path $csequiv -PathType Leaf))
            {
                Write-Host "*REMOVE*: $vbfile"
            }

            $csfile = Get-Item $csequiv
            $cstimestamp = $csfile.LastWriteTimeUtc

            if ($cstimestamp -gt $vbtimestamp)
            {          
                Write-Host "MAY need to update: $vbfile"
            }
        }
    }
}
