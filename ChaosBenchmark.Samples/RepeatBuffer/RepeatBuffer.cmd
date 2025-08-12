@ECHO OFF
SETLOCAL

ECHO net45
"%~dp0build\bin\net45\RepeatBuffer.exe"
IF NOT "%ERRORLEVEL%"=="0" GOTO :FAIL

ECHO:
ECHO net8.0
"%~dp0build\bin\net8.0\RepeatBuffer.exe"
IF NOT "%ERRORLEVEL%"=="0" GOTO :FAIL

ECHO:
ECHO Complete.
GOTO :EOF

:FAIL
    ECHO:
    ECHO Failed.
