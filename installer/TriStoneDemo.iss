; =====================================================================
; Tri-Stone Demo Setup.exe: stand-alone (owner decision 2026-11-04). Built by installer\build-demo.cmd with Inno Setup 6.
; Installs for the current Windows user, no administrator rights and no other programs needed: Tri-Stone, a small
; PostgreSQL (pgsql) and the made-up sample data (demo\demo.dump). The first start creates the private database in
; %LOCALAPPDATA%\TriStone Demo; closing Tri-Stone stops it. Can sit next to the Office version. Uninstalling deletes
; the demo's data as well (it is all made up).
; =====================================================================

; Standalone (build-standalone.cmd, owner request 2026-11-18): the same program named "Tri-Stone", with a copy of the
; home PC's real data and logins; no reset shortcut, and its data is kept when it is uninstalled.
#ifdef Standalone
  #define AppName "Tri-Stone"
  #define AppGuid "{{6E1F3A52-4C8D-4B7E-9A21-D5C7B0E84F36}"
  #define DataFolder "TriStone Local"
#else
  #define AppName "Tri-Stone Demo"
  #define AppGuid "{{B2A4D1C8-93E5-4F27-8C10-7E5D3A9F6B41}"
  #define DataFolder "TriStone Demo"
#endif
#ifndef AppVersion
  #define AppVersion "1.0.0"
#endif
#ifndef SetupName
  #define SetupName "Tri-Stone-Demo-Setup-" + AppVersion
#endif

[Setup]
AppId={#AppGuid}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher=Tri-Stone Construction Services
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputDir=output
OutputBaseFilename={#SetupName}
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
; the Test Setup (build-test.cmd) also carries the uploaded files of the PC it was made on
Source: "demo\files\*"; DestDir: "{app}\demo\files"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\TriStone.exe"
#ifndef Standalone
Name: "{group}\Reset Tri-Stone Demo (back to the sample data)"; Filename: "{app}\TriStone.exe"; Parameters: "--reset-demo"
#endif
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\TriStone.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\TriStone.exe"; Description: "Open {#AppName} now"; Flags: nowait postinstall skipifsilent

#ifndef Standalone
[UninstallDelete]
Type: filesandordirs; Name: "{localappdata}\{#DataFolder}"
#endif
