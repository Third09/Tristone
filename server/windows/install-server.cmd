@echo off
rem =====================================================================
rem Tri-Stone office server setup on a WINDOWS PC (the Linux laptop's twin: server\install-server.sh).
rem Double-click on the PC that becomes the server. It asks for administrator rights.
rem What it does: see install-server.ps1 next to this file.
rem =====================================================================
net session >nul 2>&1
if errorlevel 1 (
    powershell -NoProfile -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0install-server.ps1"
pause
