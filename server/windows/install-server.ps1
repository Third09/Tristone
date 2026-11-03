<#
  Tri-Stone office server setup on a WINDOWS PC (owner request 2026-11-03; twin of server/install-server.sh).
  Start it with install-server.cmd (asks for administrator rights). Safe to run again: it keeps existing data.

  1. Installs PostgreSQL 18 (winget, PostgreSQL.PostgreSQL.18) unless it is already installed.
  2. Opens the database to the office network only: postgresql.conf (listen on all addresses) + pg_hba.conf
     (tristone_app and postgres from the office network) + a Windows Firewall rule for port 5432 (local subnet only).
  3. Creates / updates construction_db with every numbered script (database\setup-database.ps1) and turns on
     the app's limited login tristone_app (script 022) with its password.
  4. Shares C:\TriStoneFiles as \\SERVER\TriStoneFiles for a local Windows user "tristone", and points the app at it.
  5. Nightly backups at 23:30 (database + files) into C:\TriStoneBackups, kept 14 days, optional second copy
     (C:\ProgramData\TriStone\Server\backup.json). The backup's database password is kept in a pgpass file only
     SYSTEM and Administrators can read.
  6. Keeps the PC from sleeping (a server has to stay on).
#>
$ErrorActionPreference = "Stop"
$Here = Split-Path -Parent $MyInvocation.MyCommand.Path
$Repo = (Resolve-Path (Join-Path $Here "..\..")).Path
$Db = "construction_db"
$Files = "C:\TriStoneFiles"
$Backups = "C:\TriStoneBackups"
$ServerData = "C:\ProgramData\TriStone\Server"

function Say($text) { Write-Host ""; Write-Host "== $text" -ForegroundColor Cyan }

function Ask-Password($what) {
    while ($true) {
        $a = Read-Host "  $what (12+ characters)" -AsSecureString
        $b = Read-Host "  Type it again" -AsSecureString
        $pa = [Runtime.InteropServices.Marshal]::PtrToStringBSTR([Runtime.InteropServices.Marshal]::SecureStringToBSTR($a))
        $pb = [Runtime.InteropServices.Marshal]::PtrToStringBSTR([Runtime.InteropServices.Marshal]::SecureStringToBSTR($b))
        if ($pa -ne $pb) { Write-Host "  They don't match. Try again." -ForegroundColor Yellow; continue }
        if ($pa.Length -lt 12) { Write-Host "  Too short. Use at least 12 characters." -ForegroundColor Yellow; continue }
        if ($pa -match "[`'`"]") { Write-Host "  Please don't use quote characters." -ForegroundColor Yellow; continue }
        return $pa
    }
}

$principal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) { throw "Start it with install-server.cmd (it needs administrator rights)." }

# ---------- 0. Questions ----------
Say "Tri-Stone office server setup (Windows)"
# The real office network card: the one with a gateway (not VirtualBox / VPN adapters).
$nic = Get-NetIPConfiguration | Where-Object { $_.IPv4DefaultGateway -and $_.NetAdapter.Status -eq "Up" } |
       ForEach-Object { $_.IPv4Address } | Select-Object -First 1
if (-not $nic) { throw "This PC isn't connected to a network. Plug in the office network cable and run it again." }
$ip = $nic.IPAddress
$bytes = ([Net.IPAddress]$ip).GetAddressBytes()
$mask = ([int64]4294967295 -shl (32 - $nic.PrefixLength)) -band [int64]4294967295
$netBytes = for ($i = 0; $i -lt 4; $i++) { $bytes[$i] -band (($mask -shr (24 - 8 * $i)) -band 0xFF) }
$lanGuess = ($netBytes -join ".") + "/" + $nic.PrefixLength
Write-Host "  This PC's address now: $ip (give it a fixed address in the router, e.g. 192.168.1.50)."
$lan = Read-Host "  Office network [$lanGuess]"; if (-not $lan) { $lan = $lanGuess }
$addr = Read-Host "  Server address the office PCs will use [$ip]"; if (-not $addr) { $addr = $ip }

$pgBin = Get-ChildItem "C:\Program Files\PostgreSQL\*\bin\psql.exe" -ErrorAction SilentlyContinue |
         Sort-Object { [int]$_.Directory.Parent.Name } -Descending | Select-Object -First 1
Write-Host ""
Write-Host "  Three passwords (write them down on paper, keep them safe):"
if ($pgBin) { Write-Host "  PostgreSQL is already installed: type its postgres password." }
$pgPw = Ask-Password "postgres admin password (only for setup and repairs)"
$appPw = Ask-Password "tristone_app password (typed once on each office PC)"
$filesPw = Ask-Password "files share password for user 'tristone' (typed once on each office PC)"

# ---------- 1. PostgreSQL 18 ----------
if (-not $pgBin) {
    Say "Installing PostgreSQL 18 (a few minutes)"
    winget install --id PostgreSQL.PostgreSQL.18 -e --silent --accept-package-agreements --accept-source-agreements `
        --override "--mode unattended --unattendedmodeui none --superpassword $pgPw --serverport 5432"
    if ($LASTEXITCODE -ne 0) { throw "PostgreSQL could not be installed (winget exit $LASTEXITCODE)." }
    $pgBin = Get-ChildItem "C:\Program Files\PostgreSQL\*\bin\psql.exe" | Sort-Object { [int]$_.Directory.Parent.Name } -Descending | Select-Object -First 1
}
$pgHome = $pgBin.Directory.Parent.FullName
$psql = $pgBin.FullName
$dataDir = Join-Path $pgHome "data"
$service = Get-Service "postgresql*" | Sort-Object Name -Descending | Select-Object -First 1
$env:PGPASSWORD = $pgPw
& $psql -h localhost -U postgres -d postgres -tAc "SELECT 1" | Out-Null
if ($LASTEXITCODE -ne 0) { throw "The postgres password is not right." }

# ---------- 2. Office network only ----------
Say "Opening the database to $lan only"
$conf = Join-Path $dataDir "postgresql.conf"
if (-not (Select-String -Path $conf -Pattern "Tri-Stone" -Quiet)) {
    Add-Content -Path $conf -Encoding ascii -Value "`r`n# Tri-Stone: accept connections from the office network (pg_hba.conf decides who may sign in).`r`nlisten_addresses = '*'`r`npassword_encryption = 'scram-sha-256'"
}
$hba = Join-Path $dataDir "pg_hba.conf"
if (-not (Select-String -Path $hba -Pattern "Tri-Stone office network" -Quiet)) {
    Add-Content -Path $hba -Encoding ascii -Value "`r`n# Tri-Stone office network (install-server.ps1): the app login and the admin, from the office LAN only`r`nhost    $Db    tristone_app    $lan    scram-sha-256`r`nhost    all    postgres        $lan    scram-sha-256"
}
Restart-Service $service.Name
Start-Sleep -Seconds 3
if (-not (Get-NetFirewallRule -DisplayName "Tri-Stone database (PostgreSQL)" -ErrorAction SilentlyContinue)) {
    New-NetFirewallRule -DisplayName "Tri-Stone database (PostgreSQL)" -Direction Inbound -Protocol TCP -LocalPort 5432 `
        -RemoteAddress LocalSubnet -Action Allow -Profile Any | Out-Null
}

# ---------- 3. Database + app login ----------
Say "Creating / updating the database"
& (Join-Path $Repo "database\setup-database.ps1")
& $psql -h localhost -U postgres -d $Db -q -c "ALTER ROLE tristone_app LOGIN PASSWORD '$appPw'"
if ($LASTEXITCODE -ne 0) { throw "Could not turn on the tristone_app login." }

# ---------- 4. Files share ----------
Say "Sharing $Files as \\$addr\TriStoneFiles"
$secure = ConvertTo-SecureString $filesPw -AsPlainText -Force
if (Get-LocalUser -Name "tristone" -ErrorAction SilentlyContinue) { Set-LocalUser -Name "tristone" -Password $secure }
else {
    New-LocalUser -Name "tristone" -Password $secure -FullName "Tri-Stone files" -Description "Tri-Stone shared files (install-server.ps1)" `
        -PasswordNeverExpires -UserMayNotChangePassword | Out-Null
}
New-Item -ItemType Directory -Force -Path $Files | Out-Null
icacls $Files /grant "tristone:(OI)(CI)M" /T /Q | Out-Null
if (-not (Get-SmbShare -Name "TriStoneFiles" -ErrorAction SilentlyContinue)) {
    New-SmbShare -Name "TriStoneFiles" -Path $Files -FullAccess "Administrators" -ChangeAccess "tristone" | Out-Null
}
# Windows only shares files on a Private network: the office network is one.
Get-NetConnectionProfile | Where-Object { $_.NetworkCategory -eq "Public" } | Set-NetConnectionProfile -NetworkCategory Private
Enable-NetFirewallRule -DisplayGroup "File and Printer Sharing" -ErrorAction SilentlyContinue
& $psql -h localhost -U postgres -d $Db -q -c "UPDATE app_settings SET value = '\\$addr\TriStoneFiles' WHERE key = 'file_storage_path'"

# ---------- 5. Nightly backups ----------
Say "Nightly backups at 23:30 into $Backups"
New-Item -ItemType Directory -Force -Path $ServerData, $Backups | Out-Null
Copy-Item (Join-Path $Here "tristone-backup.ps1") $ServerData -Force
$pgpass = Join-Path $ServerData "pgpass.conf"
Set-Content -Path $pgpass -Encoding ascii -Value "localhost:5432:*:postgres:$pgPw"
icacls $pgpass /inheritance:r /grant "SYSTEM:F" "Administrators:F" /Q | Out-Null
icacls $Backups /inheritance:r /grant "SYSTEM:(OI)(CI)F" "Administrators:(OI)(CI)F" /Q | Out-Null
$cfg = Join-Path $ServerData "backup.json"
if (-not (Test-Path $cfg)) {
    Set-Content -Path $cfg -Encoding utf8 -Value "{`r`n  `"CopyTo`": `"`",`r`n  `"KeepDays`": 14`r`n}"
}
$action = New-ScheduledTaskAction -Execute "powershell.exe" -Argument "-NoProfile -ExecutionPolicy Bypass -File `"$ServerData\tristone-backup.ps1`""
$trigger = New-ScheduledTaskTrigger -Daily -At "23:30"
$settings = New-ScheduledTaskSettingsSet -StartWhenAvailable -ExecutionTimeLimit (New-TimeSpan -Hours 2)
Register-ScheduledTask -TaskName "Tri-Stone nightly backup" -Action $action -Trigger $trigger -Settings $settings `
    -User "SYSTEM" -RunLevel Highest -Force | Out-Null

# ---------- 6. Stay awake ----------
powercfg /change standby-timeout-ac 0
powercfg /change hibernate-timeout-ac 0

Remove-Item Env:PGPASSWORD
Say "Done"
Write-Host @"
  Database:  $addr : 5432, database $Db
  App login: tristone_app  (password: the one you just chose)
  Files:     \\$addr\TriStoneFiles  (user tristone)
  Backups:   $Backups every night at 23:30 (a second copy: set CopyTo in $cfg, e.g. "E:\\Backups")

  On each office PC:
    1. Double-click server\connect-files.cmd (asks for the files password once).
    2. Open Tri-Stone > Database Settings: Server $addr, User tristone_app, the app password.
  Keep this PC switched on (it no longer goes to sleep when plugged in).
"@
