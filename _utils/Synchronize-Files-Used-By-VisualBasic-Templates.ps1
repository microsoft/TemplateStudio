# Overwrite all XAML files in VB directories with files from comparable C# folders. 
# XAML files should be the same for both C# & VB so this means changes can be made for C# and then easily copied for the VB templates.
# This is necessary because the way template folders are copied and filtered means that, for example, the VB templates can't point to files in the C# folders.

# This script finds all interested files in VB folders and then copies the equivalent file from the CS version
Get-ChildItem ..\templates\* -Recurse -include *.xaml, *.resw, *.md, *.png, *.description.json Package.appxmanifest | where { $_.FullName -Match "VB\\" } | % { $cs = $_.FullName -replace "VB\\", "\\"; Copy-Item $cs $_.FullName }
