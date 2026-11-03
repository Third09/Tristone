@echo off
rem Tri-Stone: put a database copy (.dump) onto this Windows office server.
rem Drag the .dump file onto this file, or double-click and type its path. See restore-database.ps1.
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0restore-database.ps1" "%~1"
pause
