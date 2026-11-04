; =====================================================================
; Tri-Stone Demo Setup.exe: stand-alone (owner decision 2026-11-04). Built by installer\build-demo.cmd with Inno Setup 6.
; Installs for the current Windows user, no administrator rights and no other programs needed: Tri-Stone, a small
; PostgreSQL (pgsql) and the made-up sample data (demo\demo.dump). The first start creates the private database in
; %LOCALAPPDATA%\TriStone Demo; closing Tri-Stone stops it. Can sit next to the Office version. Uninstalling deletes
; the demo's data as well (it is all made up).
; =====================================================================

#define AppName "Tri-Stone Demo"
#ifndef AppVersion
  #define AppVersion "1.0.0"
#endif

[Setup]
AppId={{B2A4D1C8-93E5-4F27-8C10-7E5D3A9F6B41}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher=Tri-Stone Construction Services
DefaultDirName={autopf}\Tri-Stone Demo
DefaultGroupName=Tri-Stone Demo
DisableProgramGroupPage=yes
OutputDir=output
OutputBaseFilename=Tri-Stone-Demo-Setup-{#AppVersion}
Compression=lzma2/max
SolidCompression=yes
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=lowest
UninstallDisplayIcon={app}\TriStone.exe
WizardStyle=modern
CloseApplications=yes

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Shortcuts:"

[Files]
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "edition\edition.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "demo\pgsql\*"; DestDir: "{app}\pgsql"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "demo\demo.dump"; DestDir: "{app}\demo"; Flags: ignoreversion

[Icons]
Name: "{group}\Tri-Stone Demo"; Filename: "{app}\TriStone.exe"
Name: "{group}\Reset Tri-Stone Demo (back to the sample data)"; Filename: "{app}\TriStone.exe"; Parameters: "--reset-demo"
Name: "{autodesktop}\Tri-Stone Demo"; Filename: "{app}\TriStone.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\TriStone.exe"; Description: "Open Tri-Stone Demo now"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{localappdata}\TriStone Demo"
