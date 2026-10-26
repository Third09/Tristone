<#
  Tri-Stone database setup.
  Creates the database if needed, then runs every numbered script in this folder
  (001_..., 002_..., ...) that hasn't been run yet, in order.
  Safe to run again at any time: already-applied scripts are skipped.

  Usage (from this folder):
      .\setup-database.ps1                         # localhost, construction_db
      .\setup-database.ps1 -DbHost 192.168.1.50    # another server

  Password: read from %APPDATA%\postgresql\pgpass.conf if present,
  otherwise psql asks for it.
#>
param(
    [string]$DbHost   = "localhost",
    [int]   $Port     = 5432,
    [string]$Database = "construction_db",
    [string]$User     = "postgres"
)

$ErrorActionPreference = "Stop"
$env:PGOPTIONS = "-c client_min_messages=warning"   # hide "already exists, skipping" notices

# Find psql: PATH first, then the newest PostgreSQL install under Program Files.
$psql = (Get-Command psql -ErrorAction SilentlyContinue).Source
if (-not $psql) {
    $psql = Get-ChildItem "C:\Program Files\PostgreSQL\*\bin\psql.exe" -ErrorAction SilentlyContinue |
            Sort-Object { [int]$_.Directory.Parent.Name } -Descending | Select-Object -First 1 -ExpandProperty FullName
}
if (-not $psql) { throw "psql.exe not found. Install PostgreSQL first." }

function Invoke-Psql([string]$Db, [string[]]$PsqlArgs) {
    & $psql -h $DbHost -p $Port -U $User -d $Db -v ON_ERROR_STOP=1 -q -X @PsqlArgs
    if ($LASTEXITCODE -ne 0) { throw "psql failed (exit $LASTEXITCODE)" }
}

Write-Host "Server: $DbHost`:$Port   Database: $Database" -ForegroundColor Cyan

# 1. Create the database if it doesn't exist.
$exists = Invoke-Psql "postgres" @("-t", "-A", "-c", "SELECT 1 FROM pg_database WHERE datname = '$Database'")
if ($exists -ne "1") {
    Write-Host "Creating database $Database..."
    Invoke-Psql "postgres" @("-c", "CREATE DATABASE $Database")
}

# 2. Table that remembers which scripts already ran.
Invoke-Psql $Database @("-c", @"
CREATE TABLE IF NOT EXISTS schema_versions (
    version     INT          PRIMARY KEY,
    script_name VARCHAR(200) NOT NULL,
    applied_at  TIMESTAMP    NOT NULL DEFAULT NOW()
);
"@)

$applied = @(Invoke-Psql $Database @("-t", "-A", "-c", "SELECT version FROM schema_versions") | Where-Object { $_ } | ForEach-Object { [int]$_ })

# 3. Run each new numbered script inside one transaction, then record it.
$scripts = Get-ChildItem $PSScriptRoot -Filter "*.sql" |
           Where-Object { $_.Name -match '^(\d{3})_' } |
           Sort-Object Name

foreach ($s in $scripts) {
    $version = [int]($s.Name.Substring(0, 3))
    if ($applied -contains $version) { Write-Host "  skip  $($s.Name) (already applied)"; continue }

    Write-Host "  run   $($s.Name)" -ForegroundColor Yellow
    Invoke-Psql $Database @("-1", "-f", $s.FullName)
    Invoke-Psql $Database @("-c", "INSERT INTO schema_versions (version, script_name) VALUES ($version, '$($s.Name)')")
}

Write-Host "Database is up to date." -ForegroundColor Green
