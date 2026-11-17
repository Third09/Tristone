@echo off
rem Makes installer\output\Tri-Stone-Test-Setup-<version>.exe: the stand-alone program with a COPY of this PC's
rem database (test entries removed, every password Test1234, many more materials) and this PC's uploaded files.
rem Needs PostgreSQL 18 on this PC while building (the PC running it needs nothing). This PC's database is only read.
rem Double-click, or:  build-test.cmd 1.2.0
setlocal
set VERSION=%~1
if "%VERSION%"=="" set VERSION=1.0.0
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0build.ps1" -Edition Demo -FromThisPc -Version %VERSION%
pause
