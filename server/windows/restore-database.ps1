<#
  Tri-Stone: put a database copy (.dump) onto the Windows office server. Twin of server/restore-database.sh.
    - Moving in: the home PC's real data (made there with server\export-database.cmd).
    - Repairs:   a nightly backup, e.g. C:\TriStoneBackups\2026-11-30\construction_db.dump
  Start it with restore-database.cmd (drag the .dump file onto it, or it asks for the file).
  REPLACES everything in construction_db (asks first, and saves the current database into C:\TriStoneBackups first),
  then runs any newer numbered scripts and gives tristone_app its rights again. Pictures and scans are separate:
  copy them into C:\TriStoneFiles (or unzip files.zip there).
#>
param([string]$Dump)
$ErrorActionPreference = "Stop"
$Here = Split-Path -Parent $MyInvocation.MyCommand.Path
$Repo = (Resolve-Path (Join-Path $Here "..\..")).Path
$Db = if ($env:TRISTONE_DB) { $env:TRISTONE_DB } else { "construction_db" }

if (-not $Dump) { $Dump = (Read-Host "Path of the .dump file").Trim('"') }
if (-not (Test-Path $Dump)) { throw "File not found: $Dump" }
$bin = (Get-ChildItem "C:\Program Files\PostgreSQL\*\bin\psql.exe" | Sort-Object { [int]$_.Directory.Parent.Name } -Descending | Select-Object -First 1).Directory.FullName
$psql = Join-Path $bin "psql.exe"

if (-not $env:PGPASSWORD -and -not (Test-Path "$env:APPDATA\postgresql\pgpass.conf")) {
    $s = Read-Host "postgres admin password" -AsSecureString
    $env:PGPASSWORD = [Runtime.InteropServices.Marshal]::PtrToStringBSTR([Runtime.InteropServices.Marshal]::SecureStringToBSTR($s))
}
if ($env:TRISTONE_YES -ne "YES") {
    $ok = Read-Host "This REPLACES the whole Tri-Stone database ($Db) with $(Split-Path $Dump -Leaf). Type YES to continue"
    if ($ok -ne "YES") { Write-Host "Stopped. Nothing changed."; exit 1 }
}

function Pg([string[]]$PgArgs) { & $psql -h localhost -U postgres -v ON_ERROR_STOP=1 -q -X @PgArgs; if ($LASTEXITCODE -ne 0) { throw "psql failed" } }

if ((& $psql -h localhost -U postgres -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname = '$Db'") -eq "1") {
    $backups = if ($env:TRISTONE_BACKUPS) { $env:TRISTONE_BACKUPS } else { "C:\TriStoneBackups" }
    New-Item -ItemType Directory -Force -Path $backups | Out-Null
    $safe = Join-Path $backups ("before-restore-" + (Get-Date -Format "yyyy-MM-dd-HHmm") + ".dump")
    & (Join-Path $bin "pg_dump.exe") -h localhost -U postgres -Fc -f $safe $Db
    if ($LASTEXITCODE -ne 0) { throw "Could not save the current database first. Nothing changed." }
    Write-Host "Current database saved to $safe"
    Pg @("-d", "postgres", "-c", "SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '$Db' AND pid <> pg_backend_pid()")
    Pg @("-d", "postgres", "-c", "DROP DATABASE $Db")
}
Pg @("-d", "postgres", "-c", "CREATE DATABASE $Db")
# --no-owner --no-acl: everything belongs to postgres here; the app login gets its rights again below (022).
& (Join-Path $bin "pg_restore.exe") -h localhost -U postgres --no-owner --no-acl --role=postgres -d $Db $Dump
if ($LASTEXITCODE -ne 0) { throw "pg_restore reported a problem (exit $LASTEXITCODE)." }

& (Join-Path $Repo "database\setup-database.ps1") -Database $Db
Pg @("-d", $Db, "-f", (Join-Path $Repo "database\022_app_role.sql"))

# The copy carries the home PC's files folder: point it at this server's share again.
$ip = (Get-NetIPConfiguration | Where-Object { $_.IPv4DefaultGateway -and $_.NetAdapter.Status -eq "Up" } |
       ForEach-Object { $_.IPv4Address } | Select-Object -First 1).IPAddress
$addr = if ($env:TRISTONE_ADDR) { $env:TRISTONE_ADDR } else { Read-Host "Server address the office PCs use [$ip]" }
if (-not $addr) { $addr = $ip }
Pg @("-d", $Db, "-c", "UPDATE app_settings SET value = '\\$addr\TriStoneFiles' WHERE key = 'file_storage_path'")
Write-Host "Done. Office PCs can sign in again." -ForegroundColor Green
