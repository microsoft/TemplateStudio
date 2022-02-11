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
	
REM Do a restore and release build on the solution
msbuild "%~1" /t:Restore;Rebuild /p:RestorePackagesPath="C:\Packs" /p:Configuration=Release;Platform=x86;AppxPackageSigningEnabled=false;RuntimeIdentifier=win10-x86
REM Then build the AppxBundle for the project
msbuild "%~2" /p:Configuration=Release /p:AppxBundle=Always /p:AppxBundlePlatforms="x86"
IF %ERRORLEVEL% NEQ 0 (
	GOTO ERROR
)
ECHO OFF

GOTO END

:ERROR
	EXIT 1
:END
