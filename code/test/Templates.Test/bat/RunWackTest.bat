REM #Setup MsBuild context by brute force, restore packages and build the solution
IF EXIST "%ProgramFiles(x86)%\Windows Kits\10\App Certification Kit" (
	GOTO DEV15
)
ELSE (
	GOTO ERROR
)

:DEV15
	ECHO ON
	
	"%ProgramFiles(x86)%\Windows Kits\10\App Certification Kit\appcert.exe" reset
	"%ProgramFiles(x86)%\Windows Kits\10\App Certification Kit\appcert.exe" test -appxpackagepath "%~1" -reportoutputpath "%~2"

	IF %ERRORLEVEL% NEQ 0 (
		GOTO ERROR
	)
	ECHO OFF

GOTO END

:ERROR
	EXIT 1
:END
