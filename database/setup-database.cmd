@echo off
rem Double-click to create or update the Tri-Stone database on this PC (localhost).
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0setup-database.ps1" %*
pause
