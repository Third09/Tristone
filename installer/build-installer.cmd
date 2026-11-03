@echo off
rem =====================================================================
rem Builds installer\output\Tri-Stone-Setup-<version>.exe for the office PCs.
rem Needs: the .NET 10 SDK (already used for building) and Inno Setup 6 (free: https://jrsoftware.org/isinfo.php,
rem or "winget install JRSoftware.InnoSetup"). Double-click; optional version:  build-installer.cmd 1.2.0
rem =====================================================================
setlocal
cd /d "%~dp0"
set VERSION=%~1
if "%VERSION%"=="" set VERSION=1.0.0

set ISCC=
if exist "%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe" set "ISCC=%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe"
if exist "%ProgramFiles%\Inno Setup 6\ISCC.exe" set "ISCC=%ProgramFiles%\Inno Setup 6\ISCC.exe"
if exist "%LOCALAPPDATA%\Programs\Inno Setup 6\ISCC.exe" set "ISCC=%LOCALAPPDATA%\Programs\Inno Setup 6\ISCC.exe"
if not defined ISCC (
    echo Inno Setup 6 is not installed. Get it from https://jrsoftware.org/isinfo.php
    pause
    exit /b 1
)

echo 1/2  Packaging Tri-Stone (self-contained, no .NET needed on the office PCs)...
if exist publish rmdir /s /q publish
dotnet publish ..\src\Company.Launcher -c Release -r win-x64 --self-contained true -o publish -p:OutputPath="%~dp0obj-release\\" -p:Version=%VERSION%
if errorlevel 1 (
    echo Packaging failed.
    pause
    exit /b 1
)

echo 2/2  Making Setup.exe...
"%ISCC%" /Qp /DAppVersion=%VERSION% TriStone.iss
if errorlevel 1 (
    echo Making Setup.exe failed.
    pause
    exit /b 1
)
echo.
echo Done: %~dp0output\Tri-Stone-Setup-%VERSION%.exe
pause
