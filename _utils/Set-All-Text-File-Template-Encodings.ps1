# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.
# See the LICENSE file in the project root for more information.

# This script checks that all text files in the templates folder have
# the UTF8+BOM encoding.
# This encoding allows for the use of other languages in files.
# Any files that don't have the desired encoding will be changed
# so they are encoded as desired.

#Requires -PSEdition Core

function Is-EncodedCorrectly
{
   [OutputType([Boolean])]
   [CmdletBinding()]
   Param (
     [Parameter(Mandatory = $True, ValueFromPipelineByPropertyName = $True)]
     [string]$Path
   )

   [byte[]] $byte = Get-Content -AsByteStream -Raw -Path $Path

   # EF BB BF (UTF8 + BOM)
   return $byte[0] -eq 0xEF -and $byte[1] -eq 0xBB -and $byte[2] -eq 0xBF
}

# Template paths.
$templateRoots = "..\code\TemplateStudioForWinUICs\Templates",
                 "..\code\TemplateStudioForWinUICpp\Templates",
                 "..\code\TemplateStudioForWPF\Templates",
                 "..\code\TemplateStudioForUWP\Templates",
                 "..\code\Test\TemplateStudioForWinUICs.Tests\TestData\WinUI",
                 "..\code\Test\TemplateStudioForWPF.Tests\TestData\WPF",
                 "..\code\Test\TemplateStudioForUWP.Tests\TestData\UWP"

# File extension filters.
$extensions = "*.appxmanifest",
              "*.cs",
              "*.csproj",
              "*.json",
              "*.md",
              "*.resw",
              "*.vb",
              "*.vbproj",
              "*.xaml",
              "*.xml"

foreach ($templateRoot in $templateRoots)
{
    Get-ChildItem -Path $templateRoot -Recurse -Include $extensions |
    ForEach-Object {
        if (-not (Is-EncodedCorrectly $_.FullName))
        {
            Write-Host "Changing encoding of" $_.FullName

            Get-Content $_.FullName | Out-File $_.FullName -Encoding utf8BOM
        }
    }
}