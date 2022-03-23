REM #Setup MsBuild context by brute force, restore packages and build the solution


call "%~1\Common7\Tools\VsMSBuildCmd.bat"
	
ECHO ON
REM #Nuget restore (the nuget.exe path is a param)
"%~5" restore "%~2"
msbuild "%~2" /p:Configuration=%4 /p:Platform=%3
IF %ERRORLEVEL% NEQ 0 ( 
	GOTO ERROR 
)
ECHO OFF

GOTO END

:ERROR
	EXIT 1
:END


