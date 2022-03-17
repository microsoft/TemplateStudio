
call "%~1\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" "%~2" "%~3" "%~4"
	
ECHO ON

IF %ERRORLEVEL% NEQ 0 (
	ECHO %ERRORLEVEL%
	GOTO ERROR 
)

ECHO OFF

GOTO END

:ERROR
	EXIT 1
:END
