@echo off
rem Double-click to rebuild "User Manual.docx" and "Developer Guide.docx" from docs\_source.
rem Close both files in Word first.
cd /d "%~dp0"
if not exist node_modules call npm install
node build-guides.js || goto :eof
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0to-word.ps1"
pause
