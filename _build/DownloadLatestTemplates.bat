IF EXIST "%2" (DEL %2 /S)

%ProgramFiles(x86)%\Microsoft SDKs\Azure\AzCopy\AzCopy.exe /Source:%2 /SourceKey:%3 /Dest:%4 /Y