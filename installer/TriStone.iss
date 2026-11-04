; =====================================================================
; Tri-Stone Office Setup.exe for the office PCs (Phase 6). Built by installer\build-office.cmd with Inno Setup 6.
; Installs the self-contained Tri-Stone program (no .NET needed on the PC) into Program Files, with a desktop
; and Start menu shortcut, plus edition.json: the server address and login are fixed there. The first start asks
; only for the database password (typed once on each PC, by whoever installs it; it is not inside this Setup).
; Settings stay in C:\ProgramData\TriStone when Tri-Stone is uninstalled or updated.
; =====================================================================

#define AppName "Tri-Stone"
#ifndef AppVersion
  #define AppVersion "1.0.0"
#endif
; Tests build with /DPriv=lowest (no administrator prompt).
#ifndef Priv
  #define Priv "admin"
#endif

[Setup]
AppId={{6F0C2B7E-6F3D-4A51-9B7E-2C4D9A1E5B30}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher=Tri-Stone Construction Services
DefaultDirName={autopf}\Tri-Stone
DefaultGroupName=Tri-Stone
DisableProgramGroupPage=yes
OutputDir=output
OutputBaseFilename=Tri-Stone-Office-Setup-{#AppVersion}
Compression=lzma2/max
SolidCompression=yes
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired={#Priv}
UninstallDisplayIcon={app}\TriStone.exe
WizardStyle=modern
; Close a running Tri-Stone before updating it.
CloseApplications=yes

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Shortcuts:"

[Dirs]
; Every Windows user on the PC shares the saved database password and website settings.
Name: "{commonappdata}\TriStone"; Permissions: users-modify

[Files]
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "edition\edition.json"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Tri-Stone"; Filename: "{app}\TriStone.exe"
Name: "{autodesktop}\Tri-Stone"; Filename: "{app}\TriStone.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\TriStone.exe"; Description: "Open Tri-Stone now"; Flags: nowait postinstall skipifsilent
