<#
  Builds a Tri-Stone Setup file into installer\output. Started by build-office.cmd or build-demo.cmd.
  Office: Tri-Stone-Office-Setup-<version>.exe for the office PCs, with the server address fixed in it (never the password).
  Demo:   Tri-Stone-Demo-Setup-<version>.exe, stand-alone: a small PostgreSQL + sample data inside.
  Needs: .NET 10 SDK, Inno Setup 6, and for the Demo a PostgreSQL 18 on this PC (its program files are copied, and a
  scratch database zz_demo_build is used and dropped to make the sample data). Real databases are not touched.
#>
param(
    [Parameter(Mandatory)][ValidateSet("Office", "Demo")][string]$Edition,
    [string]$Version = "1.0.0",
    [string]$ServerAddress = "192.168.1.50"
)
$ErrorActionPreference = "Stop"
$Here = Split-Path -Parent $MyInvocation.MyCommand.Path
$Repo = (Resolve-Path (Join-Path $Here "..")).Path

$iscc = @("${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe", "$env:ProgramFiles\Inno Setup 6\ISCC.exe", "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe") |
        Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $iscc) { throw "Inno Setup 6 is not installed. Get it from https://jrsoftware.org/isinfo.php" }

Write-Host "Packaging Tri-Stone (no .NET needed on the PCs)..." -ForegroundColor Cyan
$publish = Join-Path $Here "publish"
if (Test-Path $publish) { Remove-Item $publish -Recurse -Force }
& dotnet publish (Join-Path $Repo "src\Company.Launcher") -c Release -r win-x64 --self-contained true -o $publish "-p:OutputPath=$Here\obj-release\\" "-p:Version=$Version" -v quiet -nologo
if ($LASTEXITCODE -ne 0) { throw "Packaging failed." }

$editionDir = Join-Path $Here "edition"
New-Item -ItemType Directory -Force -Path $editionDir | Out-Null
$editionFile = Join-Path $editionDir "edition.json"

if ($Edition -eq "Office") {
    @{ Edition = "Office"; Host = $ServerAddress; Port = 5432; Database = "construction_db"; Username = "tristone_app" } |
        ConvertTo-Json | Set-Content -Path $editionFile -Encoding ascii
    $script = "TriStone.iss"
}
else {
    @{ Edition = "Demo" } | ConvertTo-Json | Set-Content -Path $editionFile -Encoding ascii
    $demo = Join-Path $Here "demo"

    Write-Host "Making the sample data..." -ForegroundColor Cyan
    $pgHome = (Get-ChildItem "C:\Program Files\PostgreSQL\*\bin\psql.exe" | Sort-Object { [int]$_.Directory.Parent.Name } -Descending | Select-Object -First 1).Directory.Parent.FullName
    if (-not $pgHome) { throw "PostgreSQL not found on this PC." }
    $psql = Join-Path $pgHome "bin\psql.exe"
    if (-not $env:PGPASSWORD -and -not (Test-Path "$env:APPDATA\postgresql\pgpass.conf")) {
        $s = Read-Host "This PC's postgres password" -AsSecureString
        $env:PGPASSWORD = [Runtime.InteropServices.Marshal]::PtrToStringBSTR([Runtime.InteropServices.Marshal]::SecureStringToBSTR($s))
    }
    $scratch = "zz_demo_build"
    & $psql -h localhost -U postgres -d postgres -q -c "DROP DATABASE IF EXISTS $scratch" | Out-Null
    & (Join-Path $Repo "database\setup-database.ps1") -Database $scratch | Out-Null
    & $psql -h localhost -U postgres -d $scratch -v ON_ERROR_STOP=1 -q -f (Join-Path $demo "demo_seed.sql")
    if ($LASTEXITCODE -ne 0) { throw "The sample data failed." }
    & (Join-Path $pgHome "bin\pg_dump.exe") -h localhost -U postgres -Fc -f (Join-Path $demo "demo.dump") $scratch
    if ($LASTEXITCODE -ne 0) { throw "pg_dump failed." }
    & $psql -h localhost -U postgres -d postgres -q -c "DROP DATABASE $scratch" | Out-Null

    Write-Host "Copying the small PostgreSQL..." -ForegroundColor Cyan
    $pg = Join-Path $demo "pgsql"
    if (Test-Path $pg) { Remove-Item $pg -Recurse -Force }
    New-Item -ItemType Directory -Force -Path "$pg\bin", "$pg\lib" | Out-Null
    foreach ($exe in "postgres", "initdb", "pg_ctl", "psql", "pg_restore") { Copy-Item "$pgHome\bin\$exe.exe" "$pg\bin" }
    Copy-Item "$pgHome\bin\*.dll" "$pg\bin"
    Copy-Item "$pgHome\lib\*.dll" "$pg\lib"
    # The Microsoft runtime PostgreSQL needs (a PC may not have it; allowed to travel next to the program).
    foreach ($dll in "vcruntime140.dll", "vcruntime140_1.dll", "msvcp140.dll") {
        if (Test-Path "$env:WINDIR\System32\$dll") { Copy-Item "$env:WINDIR\System32\$dll" "$pg\bin" }
    }
    Copy-Item "$pgHome\share" "$pg\share" -Recurse
    Remove-Item "$pg\share\locale" -Recurse -Force -ErrorAction SilentlyContinue
    Copy-Item "$pgHome\server_license.txt" $pg -ErrorAction SilentlyContinue
    $script = "TriStoneDemo.iss"
}

Write-Host "Making the Setup file..." -ForegroundColor Cyan
& $iscc /Qp "/DAppVersion=$Version" (Join-Path $Here $script)
if ($LASTEXITCODE -ne 0) { throw "Making the Setup file failed." }
Get-ChildItem (Join-Path $Here "output") -Filter "*$Edition*$Version*" | ForEach-Object {
    Write-Host ("Done: {0} ({1:N0} MB)" -f $_.FullName, ($_.Length / 1MB)) -ForegroundColor Green
}
