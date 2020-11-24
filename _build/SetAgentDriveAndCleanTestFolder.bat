ECHO OFF

ECHO VALUE BEFORE: AGENT_DRIVE=%AGENT_DRIVE%
ECHO VALUE BEFORE: Agent.Drive=%Agent.Drive%

SET AGENT_DRIVE=%CD:~0,2%
SET Agent.Drive=%CD:~0,2%

ECHO VALUE AFTER: AGENT_DRIVE=%AGENT_DRIVE%
ECHO VALUE AFTER: Agent.Drive=%Agent.Drive%

IF EXIST "%AGENT_DRIVE%\UIT" ( rd "%AGENT_DRIVE%\UIT" /s /q )
IF EXIST "%userprofile%\AppData\Local\Temp\WTSTempGeneration" ( rd "%userprofile%\AppData\Local\Temp\WTSTempGeneration" /s /q )
