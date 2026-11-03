@echo off
rem =====================================================================
rem Tri-Stone: let THIS office PC open the scans and pictures on the office server.
rem Double-click once on each office PC. Windows then remembers the files password
rem (Credential Manager), so Tri-Stone can open and save files in \\SERVER\TriStoneFiles.
rem =====================================================================
setlocal
set /p SERVER=Server address [192.168.1.50]:
if "%SERVER%"=="" set SERVER=192.168.1.50
echo Type the files password (user tristone). It is not shown while you type.
powershell -NoProfile -Command "$p = Read-Host -AsSecureString 'Files password'; $c = New-Object System.Management.Automation.PSCredential('tristone', $p); cmdkey /add:%SERVER% /user:tristone /pass:$($c.GetNetworkCredential().Password) | Out-Null; if (Test-Path '\\%SERVER%\TriStoneFiles') { 'Connected: \\%SERVER%\TriStoneFiles' } else { 'Could not open \\%SERVER%\TriStoneFiles. Check the address, the password and that the server is on.' }"
pause
