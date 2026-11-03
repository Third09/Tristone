<#
  Tri-Stone nightly backup on a Windows server (installed into C:\ProgramData\TriStone\Server by install-server.ps1;
  scheduled task "Tri-Stone nightly backup", 23:30, runs as SYSTEM). Twin of server/tristone-backup.sh.
  Makes C:\TriStoneBackups\YYYY-MM-DD\ with
    construction_db.dump   the whole database (pg_dump custom format; restore with restore-database.cmd)
    files.zip              the scans and pictures (C:\TriStoneFiles)
  Keeps KeepDays days and copies each backup to CopyTo (e.g. a USB drive) when set (backup.json).
  Run it by hand any time (as administrator):  powershell -ExecutionPolicy Bypass -File tristone-backup.ps1
#>
$ErrorActionPreference = "Stop"
$ServerData = "C:\ProgramData\TriStone\Server"
$Backups = if ($env:TRISTONE_BACKUPS) { $env:TRISTONE_BACKUPS } else { "C:\TriStoneBackups" }
$Files = if ($env:TRISTONE_FILES) { $env:TRISTONE_FILES } else { "C:\TriStoneFiles" }
$Log = Join-Path $Backups "backup.log"
$cfg = @{ CopyTo = ""; KeepDays = 14 }
$cfgFile = Join-Path $ServerData "backup.json"
if (Test-Path $cfgFile) { (Get-Content $cfgFile -Raw | ConvertFrom-Json).PSObject.Properties | ForEach-Object { $cfg[$_.Name] = $_.Value } }

function Log($text) { "$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') $text" | Tee-Object -FilePath $Log -Append }

New-Item -ItemType Directory -Force -Path $Backups | Out-Null
try {
    $pgDump = Get-ChildItem "C:\Program Files\PostgreSQL\*\bin\pg_dump.exe" | Sort-Object { [int]$_.Directory.Parent.Name } -Descending | Select-Object -First 1
    if (-not $env:PGPASSFILE) { $env:PGPASSFILE = Join-Path $ServerData "pgpass.conf" }
    $out = Join-Path $Backups (Get-Date -Format "yyyy-MM-dd")
    New-Item -ItemType Directory -Force -Path $out | Out-Null
    Log "backup start"
    & $pgDump.FullName -h localhost -U postgres -w -Fc -f (Join-Path $out "construction_db.dump") construction_db
    if ($LASTEXITCODE -ne 0) { throw "pg_dump failed (exit $LASTEXITCODE)" }
    $zip = Join-Path $out "files.zip"
    if (Test-Path $zip) { Remove-Item $zip }
    if (Get-ChildItem $Files -Recurse -File -ErrorAction SilentlyContinue | Select-Object -First 1) {
        Compress-Archive -Path (Join-Path $Files "*") -DestinationPath $zip -CompressionLevel Optimal
    }
    $size = "{0:N1} MB" -f ((Get-ChildItem $out -File | Measure-Object Length -Sum).Sum / 1MB)
    Log "made $out ($size)"

    # Old backups out.
    $limit = (Get-Date).AddDays(-[int]$cfg.KeepDays)
    Get-ChildItem $Backups -Directory | Where-Object { $_.LastWriteTime -lt $limit } | Remove-Item -Recurse -Force

    # A second copy somewhere else (a backup on the same PC is lost with the PC).
    if ($cfg.CopyTo) {
        if (Test-Path $cfg.CopyTo) {
            $dest = Join-Path $cfg.CopyTo "tristone-backups"
            New-Item -ItemType Directory -Force -Path $dest | Out-Null
            Copy-Item $out $dest -Recurse -Force
            Get-ChildItem $dest -Directory | Where-Object { $_.LastWriteTime -lt $limit } | Remove-Item -Recurse -Force
            Log "copied to $($cfg.CopyTo)"
        }
        else { Log "WARNING: $($cfg.CopyTo) not found (USB drive not plugged in?). Only the copy on this PC was made." }
    }
    Log "backup done"
}
catch { Log "BACKUP FAILED: $($_.Exception.Message)"; exit 1 }
