IF EXIST "C:\Temp\Templates.mstx" (DEL "C:\Temp\Templates.mstx" /S)

"%ProgramFiles(x86)%\Microsoft SDKs\Azure\AzCopy\AzCopy.exe" /Source:%1 /SourceKey:%3 /Dest:"C:\Temp\" /Pattern:"Templates.mstx" /Y

XCOPY "C:\Temp\Templates.mstx" %2 /Y
