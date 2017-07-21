# ******************************************************************
# Copyright (c) Microsoft. All rights reserved.
# This code is licensed under the MIT License (MIT).
# THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
# INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
# IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
# DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
# TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
# THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
# ******************************************************************

# This script checks that all text files in the templates folder have
# the UTF8+BOM encoding.
# This encoding allows for the use of other languages in files.
# Any files that don't have the desired encoding will be changed
#  so they are encoded as desired.

function Is-EncodedCorrectly
{
   [OutputType([Boolean])]
   [CmdletBinding()]
   Param (
     [Parameter(Mandatory = $True, ValueFromPipelineByPropertyName = $True)]
     [string]$Path
   )

   [byte[]]$byte = get-content -Encoding byte -ReadCount 3 -TotalCount 3 -Path $Path

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

Get-ChildItem -Path ..\templates -Recurse -Include *.appxmanifest, *.cs, *.csproj, *.json, *.md, *.resw, *.vb, *.vbproj, *.xaml, *.xml |
ForEach-Object {
    if (-NOT (Is-EncodedCorrectly $_.FullName))
    {
       Write-Host "Changing encoding of" $_.FullName;

       # the encoding value of 'utf8' does include the Byte Order Mark (signature)
       (Get-Content $_.FullName) | Out-File $_.FullName -Encoding utf8;
    }
}
