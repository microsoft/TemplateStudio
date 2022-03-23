REM #Setup MsBuild context by brute force, restore packages and build the solution

call "%~1\Common7\Tools\VsMSBuildCmd.bat"
	
ECHO ON
	
msbuild "%~2" /t:Restore;Rebuild /p:RestorePackagesPath="C:\Packs" /p:Configuration=%4;Platform=%3;AppxPackageSigningEnabled=false
IF %ERRORLEVEL% NEQ 0 ( 
	GOTO ERROR 
)
ECHO OFF

GOTO END

:ERROR
	EXIT 1
:END
