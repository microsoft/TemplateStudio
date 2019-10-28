REM #Setup MsBuild context by brute force, restore packages and build the solution
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017" (
	GOTO DEV15
)
ELSE (
	GOTO ERROR
)

:DEV15
	IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community" (
		call "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\Common7\Tools\VsMSBuildCmd.bat"
	)
	IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional" (
		call "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\Tools\VsMSBuildCmd.bat"
	)
	IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise" (
		call "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\VsMSBuildCmd.bat"
	)

	ECHO ON
	
	REM Do a restore and release build on the solution
	msbuild "%~1" /t:Restore;Rebuild /p:RestorePackagesPath="C:\Packs" /p:Configuration=Release;Platform=x86;AppxPackageSigningEnabled=false
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
