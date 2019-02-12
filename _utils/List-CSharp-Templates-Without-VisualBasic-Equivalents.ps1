# This script will list all the C# template folders that do not have VB equivalents.
# Any output indicates missing VB templates.

# Get list of all templates
$allTemplates = Get-ChildItem ..\templates\* -Recurse -include template.json | where { $_.FullName -notmatch "\\templates\\Uwp\\Test\\" } | % { Write-Output $_.FullName }
Foreach ($t in $allTemplates)
{
    if ($t -like '*_shared\Page.AddConnectedAnimationService*')
    {
        # This is a shared template but only used by Prism & Caliburn.Micro so doesn't need a VB equivalent
        continue
    }

    # Ignore VB ones
    if ($t -notmatch "._VB\\")
    {
        $hasVbEquivalent = $false;

        # Search through all templates
        foreach ($u in $allTemplates)
        {
            # Now only interested in VB ones
            if ($u -match "._VB\\")
            {
                # See if it matches the CS one we're looking for
                $without = $u -replace "._VB\\", "\";

                if ($t -eq $without)
                {
                    # If it matches that means the CS version has a VB equivalent so we can move on
                    $hasVbEquivalent = $true;
                    break;
                }
            }
        }

        # Output the name of the template that needs a VB equivalent
        # Exclude CaliburnMicro & Prism templates while not supporting VB (yet)
        if (-not $hasVbEquivalent -and $t -notmatch "Caliburn" -and $t -notmatch "Prism")
        {
            # This will be the path of the folder that needs a VB version
            $templateName = $t -replace "\\.template.config\\template.json", ""
            Write-Output $templateName
        }
    }
}
