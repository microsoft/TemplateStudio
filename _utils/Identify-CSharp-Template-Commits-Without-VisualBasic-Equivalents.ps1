# To run this file, open PoshGit at the root of the repo and run ".\_utils\Identify-CSharp-Template-Commits-Without-VisualBasic-Equivalents.ps1"

## This script will tell us all CS template files that have been changed but their VB equivalents haven't
## May include false positives (CS file changed but VB one doesn't need changing)
## May also include false negatives (VB file changed but not to be semantically equivalent to the CS one)

## This script cannot be perfect in identifying everything. It should be used in combination with the script that looks at file last-modified times.

# last param is the SHA of the commit to check from (this should normally be the last release)
$files = git diff --name-only 7a4c807bf03ad0b3f8fff045752e1b3375603b67

function Get-CsEquivalentFile($vbfile)
{
    # Need to support local and remote file paths
    return $vbfile -replace "._VB/", "/" `
                   -replace "._VB\\", "\" `
                   -replace ".vb", ".cs"`
                   -replace "My Project", "Properties"
}

$csfiles = @()
$vbfiles = @()

# Sort files interested in
Foreach ($file in $files)
{
    if ($file.StartsWith("templates"))
    {
        if ($file.EndsWith(".xaml") -or 
            $file.EndsWith(".md") -or 
            $file.EndsWith(".png") -or 
            $file.EndsWith(".resw") -or 
            $file.EndsWith(".rd.xml") -or 
            $file.EndsWith(".template.json") -or 
            $file.Contains("_catalog"))
        {     
            # Ignore non-code files
        }
        else
        {
            if ($file -match "VB/")
            {
                $vbfiles += $file
            }
            else
            {
                $csfiles += $file
            }
        }
    }
}

# Need a copy of all VB files on disk for comparison as it's not possible to easily get the VB version of a CS file name
$allTemplates = Get-ChildItem ..\* -Recurse -include template.json | % { Write-Output $_.FullName }
$vbfilesOnDisk = @()
Foreach ($template in $allTemplates)
{
    if ($template -match "._VB\\")
    {
        $vbfilesOnDisk += $template
    }
}

Foreach ($csfile in $csfiles)
{    
    # Check if file has been committed
    $vbEquivChanged = $false;

    Foreach ($vbfile in $vbfiles)
    {
        $equiv = Get-CsEquivalentFile($vbfile)

        #If (Get-CsEquivalentFile($vbfile) -eq $csfile)
        If ($equiv -eq $csfile)
        {
            $vbEquivChanged = $true;
            #Write-Host "change assumed: $equiv "
            Break
        }
    }

    # If Vb version hasn't been committed see if one exists on disk - If it doesn't that will mean the VB version doesn't exist
    if ($vbEquivChanged -eq $false)
    {
        Foreach ($vbfile in $vbfilesOnDisk)
        {
            $equiv = Get-CsEquivalentFile($vbfile)
            If ($equiv -eq $csfile)
            {
                Write-Host "MAY need to apply changes from: $csfile"
                Break
            }
        }
    }
}
