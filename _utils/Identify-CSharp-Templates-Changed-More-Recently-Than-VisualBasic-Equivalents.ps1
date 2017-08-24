# Firstly check file system and detect updates based on last modified time

function Get-CsEquivalentFile($vbfile)
{
    return $vbfile -replace "VB\\", "\" `
                   -replace ".vb", ".cs"`
                   -replace "My Project", "Properties"
}

# Get list of all templates
$allTemplates = Get-ChildItem ..\templates\* -Recurse -include template.json | % { Write-Output $_.FullName }
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
                #Write-Output $cstimestamp
                #Write-Output $vbtimestamp
            }
        }
    }
}

# This may not get everything - not all changes to CS will require a VB change. + VB files may have been changed for reasons other than semantic sync

# TODO Also do equivalent of above but by querying GIT directly to find changes
