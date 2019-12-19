
@ECHO OFF

IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" (
ECHO ON
	"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" "%~1" "%~2" "%~3"
ECHO OFF
) ELSE IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" (
ECHO ON
	"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" "%~1" "%~2" "%~3"
ECHO OFF
) ELSE IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" (
ECHO ON
	"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" "%~1" "%~2" "%~3"
ECHO OFF
) ELSE (
ECHO ON
	ECHO vstest.console.exe not found!
)

IF %ERRORLEVEL% NEQ 0 (
	ECHO %ERRORLEVEL%
	GOTO ERROR 
)

ECHO OFF

GOTO END

:ERROR
	EXIT 1
:END
