; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Katarai"
#define MyAppVersion "0.7.15"
#define MyAppPublisher "Chillisoft Solution Services (Pty) Ltd"
#define MyAppURL "http://www.chillisoft.co.za"
#define MyAppExeName "Katarai.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{7312122A-95D5-4B60-B079-9F31DB914F6A}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\Chillisoft\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=installer
OutputBaseFilename=Katarai.{#MyAppVersion}.setup
SetupIconFile=lib\Katarai.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkablealone
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkablealone; OnlyBelowVersion: 0,6.1

[Files]
Source: "bin\release\Katarai.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\Engine.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\Hardcodet.Wpf.TaskbarNotification.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\Ionic.Zip.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\Katarai.Runner.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\log4net.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\Splunk.Client.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\nunit.framework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\Katarai.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\Katarai.Controls.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\Katarai.Utils.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\Xceed.Wpf.Toolkit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\UpdateBootstrapperLibrary.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\OxyPlot.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\OxyPlot.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\HtmlRenderer.WPF.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\release\HtmlRenderer.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
Root: "HKLM64"; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "Katarai"; ValueData: "{app}\{#MyAppExeName}"; Flags: uninsdeletevalue createvalueifdoesntexist; Check: (IsWin64);
Root: "HKLM32"; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "Katarai"; ValueData: "{app}\{#MyAppExeName}"; Flags: uninsdeletevalue createvalueifdoesntexist; Check: (not IsWin64);

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
