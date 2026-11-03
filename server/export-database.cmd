@echo off
rem =====================================================================
rem Tri-Stone: make a copy of THIS PC's database to move it to the office server.
rem Double-click on the home PC (the one with the real data). It writes
rem   construction_db.dump   next to this file.
rem Then copy that file and the C:\TriStoneFiles folder to the server
rem (User Manual: "Moving the data to the office server").
rem Asks for the postgres password unless it is in %APPDATA%\postgresql\pgpass.conf.
rem =====================================================================
setlocal
set PGDUMP=
for /d %%v in ("C:\Program Files\PostgreSQL\*") do if exist "%%v\bin\pg_dump.exe" set "PGDUMP=%%v\bin\pg_dump.exe"
if not defined PGDUMP (
    echo pg_dump.exe not found. Is PostgreSQL installed on this PC?
    pause
    exit /b 1
)
echo Copying the database construction_db ...
"%PGDUMP%" -h localhost -U postgres -Fc -f "%~dp0construction_db.dump" construction_db
if errorlevel 1 (
    echo Something went wrong. Nothing was made.
    pause
    exit /b 1
)
echo.
echo Done: %~dp0construction_db.dump
echo Copy it to the server, together with the C:\TriStoneFiles folder.
pause
