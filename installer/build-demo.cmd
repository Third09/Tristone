@echo off
rem Makes installer\output\Tri-Stone-Demo-Setup-<version>.exe: stand-alone, with its own database and sample data.
rem Needs PostgreSQL 18 on this PC while building (the PC running the Demo needs nothing).
rem Double-click, or:  build-demo.cmd 1.2.0
setlocal
set VERSION=%~1
if "%VERSION%"=="" set VERSION=1.0.0
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0build.ps1" -Edition Demo -Version %VERSION%
pause
