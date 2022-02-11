REM #Setup MsBuild context by brute force, restore packages and build the solution

IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Community" (
	call "%ProgramFiles%\Microsoft Visual Studio\2022\Community\Common7\Tools\VsMSBuildCmd.bat"
)
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Professional" (
	call "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\Common7\Tools\VsMSBuildCmd.bat"
)
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise" (
	call "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsMSBuildCmd.bat"
)
IF EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Preview" (
	call "%ProgramFiles%\Microsoft Visual Studio\2022\Preview\Common7\Tools\VsMSBuildCmd.bat"
)
	
ECHO ON
	
msbuild "%~1" /t:Restore;Rebuild /p:RestorePackagesPath="C:\Packs" /p:Configuration=%3;Platform=%2;RuntimeIdentifier=win10-x86
IF %ERRORLEVEL% NEQ 0 ( 
	GOTO ERROR 
)
ECHO OFF

GOTO END

:ERROR
	EXIT 1
:END


