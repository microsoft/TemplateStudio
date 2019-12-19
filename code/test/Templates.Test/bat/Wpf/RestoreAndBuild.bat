REM #Setup MsBuild context by brute force, restore packages and build the solution

IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community" (
	call "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\Common7\Tools\VsMSBuildCmd.bat"
)
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional" (
	call "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\Common7\Tools\VsMSBuildCmd.bat"
)
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise" (
	call "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\VsMSBuildCmd.bat"
)
	
ECHO ON
	
msbuild "%~1" /t:Restore;Rebuild /p:RestorePackagesPath="C:\Packs" /p:Configuration=%3;Platform=%2
IF %ERRORLEVEL% NEQ 0 ( 
	GOTO ERROR 
)
ECHO OFF

GOTO END

:ERROR
	EXIT 1
:END


