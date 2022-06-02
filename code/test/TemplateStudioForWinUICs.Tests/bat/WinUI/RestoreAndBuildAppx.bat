REM #Setup MsBuild context by brute force, restore packages and build the solution

call "%~1\Common7\Tools\VsMSBuildCmd.bat"

ECHO ON

REM Do a restore and release build on the solution
msbuild "%~2" /t:Restore;Rebuild /p:RestorePackagesPath="C:\Packs" /p:Configuration=Release;Platform=x86;AppxPackageSigningEnabled=false;RuntimeIdentifier=win10-x86
REM Then build the AppxBundle for the project
msbuild "%~3" /p:Configuration=Release;Platform=x86 /p:AppxBundle=Always /p:AppxBundlePlatforms="x86" /p:GenerateAppxPackageOnBuild=true /p:UapAppxPackageBuildMode=StoreUpload /p:AppxPackageDir="%~4"
IF %ERRORLEVEL% NEQ 0 (
	GOTO ERROR
)
ECHO OFF

GOTO END

:ERROR
	EXIT 1
:END
