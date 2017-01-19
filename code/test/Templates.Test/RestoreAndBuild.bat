#Setup MsBuild context by brute force, restore packages and build the solution
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017" (
	GOTO DEV15
)
ELSE (
	GOTO DEV14
)

:DEV15
	call "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\Common7\Tools\VsMSBuildCmd.bat"
	call "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\Tools\VsMSBuildCmd.bat"
	call "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\VsMSBuildCmd.bat"
	msbuild "%~1" /t:Restore;Rebuild /p:Configuration=%3;Platform=%2;AppxPackageSigningEnabled=false
GOTO END

:DEV14
	call "%ProgramFiles(x86)%\Microsoft Visual Studio 14.0\Common7\Tools\VsMSBuildCmd.bat"
	..\..\..\..\..\..\_tools\nuget.exe restore
	msbuild "%~1" /t:Rebuild /p:Configuration=%3;Platform=%2;AppxPackageSigningEnabled=false
GOTO END


:END


