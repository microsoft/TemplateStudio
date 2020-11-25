# This script will list all the C# template folders that do not have VB equivalents.
# Any output indicates missing VB templates.

# Get list of all templates
$allTemplates = Get-ChildItem ..\templates\* -Recurse -include template.json | where { $_.FullName -notmatch "\\templates\\Uwp\\SC\\" } | where { $_.FullName -notmatch "\\templates\\Wpf\\" } | where { $_.FullName -notmatch "\\templates\\WinUI\\" } | % { Write-Output $_.FullName }
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
    else
    {
        # Check for VB templates without CS versions - can happen when CS ones are moved/changed
        $hasCsEquivalent = $false;

        # Get name of CS equivalent
        $without = $t -replace "._VB\\", "\";

        # Search through all templates
        foreach ($u in $allTemplates)
        {
            if ($u -eq $without)
            {
                # If it matches that means the VB version has a CS equivalent so we can move on
                $hasCsEquivalent = $true;
                break;
            }
        }

        # Output the name of the template without a CS equivalent
        # Exclude CaliburnMicro & Prism templates as they don't support VB
        if (-not $hasCsEquivalent -and $t -notmatch "Caliburn" -and $t -notmatch "Prism")
        {
            # This will be the path of the folder that needs a VB version
            $templateName = $t -replace "\\.template.config\\template.json", ""
            Write-Output "$templateName Has no CS version!"
        }

    }
}
