@echo off
rem Makes installer\output\Tri-Stone-Setup-<version>.exe: Tri-Stone running on its own on any Windows PC, with a COPY
rem of this PC's database (test entries removed, history cleared) and its uploaded files. Logins and passwords stay
rem exactly as they are on this PC. No "Test as...", no sample extras; it never changes the live website.
rem Needs PostgreSQL 18 on this PC while building (the PC running it needs nothing). This PC's database is only read.
rem Double-click, or:  build-standalone.cmd 1.0.0
setlocal
set VERSION=%~1
if "%VERSION%"=="" set VERSION=1.0.0
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0build.ps1" -Edition Standalone -Version %VERSION%
pause
