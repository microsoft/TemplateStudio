REM #Setup MsBuild context by brute force, restore packages and build the solution

call "%~1\Common7\Tools\VsMSBuildCmd.bat"

ECHO ON
REM #Nuget restore (the nuget.exe path is a param)
"%~4" restore "%~2"
REM Do a restore and release build on the solution
msbuild "%~2" /p:Configuration=Release /p:Platform=x86;AppxPackageSigningEnabled=false
REM Then build the AppxBundle for the project
msbuild "%~3" /p:Configuration=Release /p:AppxBundle=Always /p:AppxBundlePlatforms="x86"
IF %ERRORLEVEL% NEQ 0 (
	GOTO ERROR
)
ECHO OFF

GOTO END

:ERROR
	EXIT 1
:END
