IF EXIST "%2" (DEL %2 /S)

%ProgramFiles(x86)%\Microsoft SDKs\Azure\AzCopy\AzCopy.exe /Source:%1 /SourceKey:%3 /Dest:%2 /Y