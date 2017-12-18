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


# This script updates all the localized string values in Prism template files from their C# equivalents.
# This is needed as the files can't just be copied as they have Prism specific content.
[string[]] $templatesPath = "..\templates\Pages","..\templates\Features";

Get-ChildItem $templatesPath -Recurse -include *template.json | where { $_.FullName -Match ".Prism\\" } |  % { 

    $prismFile = $_.FullName;
    $csFile = $_.FullName -replace ".Prism\\", "\\"; 

    $prismJson = (Get-Content $prismFile) -join "`n" | ConvertFrom-Json;
    $csJson = (Get-Content $csFile) -join "`n" | ConvertFrom-Json;

    $prismEquivIdentity = $prismJson.identity -replace ".Prism", "";

    $somethingWasChanged = $false;

    if ($prismEquivIdentity -eq $csJson.identity)
    {
        # Not every template contains "name" or "description" properties so only look at those that do.
        if (($prismJson.PSobject.Properties.name -match "name") -and ($csJson.PSobject.Properties.name -match "name"))
        {
            if ($prismJson.name -ne $csJson.name)
            {
                $prismJson.name = $csJson.name
                $somethingWasChanged = $true;
            }
        }

        if (($prismJson.PSobject.Properties.name -match "description") -and ($csJson.PSobject.Properties.name -match "description"))
        {
            if ($prismJson.description -ne $csJson.description)
            {
                $prismJson.description = $csJson.description
                $somethingWasChanged = $true;
            }
        }

        if ($somethingWasChanged)
        {
            $prismJson | ConvertTo-Json | Format-Json | Out-File $prismFile -Encoding utf8;
        }
    }
}

# This script replace all description.md files in Prism template files from their C# equivalents.
Get-ChildItem $templatesPath -Recurse -include *description.md | where { $_.FullName -Match ".Prism\\" } | % { 
    $cs = $_.FullName -replace ".Prism\\", "\\"; 
    Copy-Item $cs $_.FullName 
}