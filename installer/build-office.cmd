@echo off
rem Makes installer\output\Tri-Stone-Office-Setup-<version>.exe for the office PCs.
rem The server address is fixed inside it (the password is not: it is typed once on each PC).
rem Double-click for 192.168.1.50, or:  build-office.cmd 1.2.0 192.168.1.60
setlocal
set VERSION=%~1
if "%VERSION%"=="" set VERSION=1.0.0
set ADDRESS=%~2
if "%ADDRESS%"=="" set ADDRESS=192.168.1.50
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0build.ps1" -Edition Office -Version %VERSION% -ServerAddress %ADDRESS%
pause
