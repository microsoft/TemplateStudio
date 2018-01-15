# Overwrite all XAML files in VB directories with files from comparable C# folders. 
# XAML files should be the same for both C# & VB so this means changes can be made for C# and then easily copied for the VB templates.
# This is necessary because the way template folders are copied and filtered means that, for example, the VB templates can't point to files in the C# folders.

# This script finds all interested files in VB folders and then copies the equivalent file from the CS version
Get-ChildItem ..\templates\* -Recurse -include *.xaml, *.resw, *.md, *.png, Package.appxmanifest | where { $_.FullName -Match "VB\\" -and ($_.FullName -notmatch "\\templates\\test\\") } | % { $cs = $_.FullName -replace "VB\\", "\\"; Copy-Item $cs $_.FullName }

# This script handles project file postactions that add 3rd party references
Get-ChildItem ..\templates\* -Recurse -include _postaction.vbproj | where { $_.FullName -notmatch "\\templates\\test\\" } | % { $cs = $_.FullName -replace "VB\\", "\\"; $cs = $cs -replace ".vbproj", ".csproj"; Copy-Item $cs $_.FullName }

# Formats JSON in a nicer format than the built-in ConvertTo-Json does.
# This is based on code from https://github.com/PowerShell/PowerShell/issues/2736 and will be built into PS6.0
function Format-Json([Parameter(Mandatory, ValueFromPipeline)][String] $json) {
  $indent = 0;
  ($json -Split '\n' |
    % {
      if ($_ -match '[\}\]]') {
        # This line contains  ] or }, decrement the indentation level
        $indent--
      }
      $line = (' ' * $indent * 4) + $_.TrimStart().Replace(':  ', ': ')
      if ($_ -match '[\{\[]') {
        # This line contains [ or {, increment the indentation level
        $indent++
      }
      $line
  }) -Join "`n"
}


# This script updates all the localized string values in VB template files from their C# equivalents.
# This is needed as the files can't just be copied as they have VB specific content.
Get-ChildItem ..\templates\* -Recurse -include *template.json | where { $_.FullName -Match "VB\\" -and ($_.FullName -notmatch "\\templates\\test\\") } |  % { 

    $vbFile = $_.FullName;
    $csFile = $_.FullName -replace "VB\\", "\\"; 

    Try
    {
    $vbJson = (Get-Content $vbFile) -join "`n" | ConvertFrom-Json;
    }
    Catch
    {
        Write-Output $vbFile;
    }
    $csJson = (Get-Content $csFile) -join "`n" | ConvertFrom-Json;

    $vbEquivIdentity = $vbJson.identity -replace ".VB", "";

    $somethingWasChanged = $false;

    if ($vbEquivIdentity -eq $csJson.identity)
    {
        # Not every template contains "name" or "description" properties so only look at those that do.
        if (($vbJson.PSobject.Properties.name -match "name") -and ($csJson.PSobject.Properties.name -match "name"))
        {
            if ($vbJson.name -ne $csJson.name)
            {
                $vbJson.name = $csJson.name
                $somethingWasChanged = $true;
            }
        }

        if (($vbJson.PSobject.Properties.name -match "description") -and ($csJson.PSobject.Properties.name -match "description"))
        {
            if ($vbJson.description -ne $csJson.description)
            {
                $vbJson.description = $csJson.description
                $somethingWasChanged = $true;
            }
        }

        if ($somethingWasChanged)
        {
            $vbJson | ConvertTo-Json | Format-Json | Out-File $vbFile -Encoding utf8;
        }
    }
}
