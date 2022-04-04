# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.
# See the LICENSE file in the project root for more information.

# This script checks that all text files in the templates folder have
# the UTF8+BOM encoding.
# This encoding allows for the use of other languages in files.
# Any files that don't have the desired encoding will be changed
#  so they are encoded as desired.

#Requires -PSEdition Core

function Is-EncodedCorrectly
{
   [OutputType([Boolean])]
   [CmdletBinding()]
   Param (
     [Parameter(Mandatory = $True, ValueFromPipelineByPropertyName = $True)]
     [string]$Path
   )

   [byte[]]$byte = Get-Content -AsByteStream -Raw -Path $Path

   # EF BB BF (UTF8 + BOM)
   if ( $byte[0] -eq 0xef -and $byte[1] -eq 0xbb -and $byte[2] -eq 0xbf )
   {
       return $true 
   }
   else
   { 
       return $false
   }
}

# Filtering is done by file extension. We care about:
# *.appxmanifest
# *.cs
# *.csproj
# *.json
# *.md
# *.resw
# *.vb
# *.vbproj
# *.xaml
# *.xml

foreach ($templateRoot in "..\code\TemplateStudioForUWP\Templates", "..\code\TemplateStudioForWinUICpp\Templates", "..\code\TemplateStudioForWinUICs\Templates", "..\code\TemplateStudioForWPF\Templates", "..\code\Test\TemplateStudioForUWP.Tests\TestData\UWP", "..\code\Test\TemplateStudioForWinUICs.Tests\TestData\WinUI", "..\code\Test\TemplateStudioForWPF.Tests\TestData\WPF")
{
Get-ChildItem -Path $templateRoot -Recurse -Include *.appxmanifest, *.cs, *.csproj, *.json, *.md, *.resw, *.vb, *.vbproj, *.xaml, *.xml |
ForEach-Object {
    if (-NOT (Is-EncodedCorrectly $_.FullName))
    {
       Write-Host "Changing encoding of" $_.FullName;

       # the encoding value of 'utf8' does include the Byte Order Mark (signature)
       (Get-Content $_.FullName) | Out-File $_.FullName -Encoding utf8;
    }
}
}