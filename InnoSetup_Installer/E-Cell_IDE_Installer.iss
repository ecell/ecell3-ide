[Setup]
PrivilegesRequired=poweruser
AppComments=E-Cell IDE
InternalCompressLevel=max
Compression=lzma/ultra
VersionInfoTextVersion=E-Cell IDE alpha3
EnableDirDoesntExistWarning=Yes
WizardSmallImageFile=ecell-logo1.bmp
WizardImageFile=test.bmp
ChangesEnvironment=Yes
AppPublisherURL=http://sourceforge.net/project/showfiles.php?group_id=72485
AppPublisher=Keio University and Mitsubishi Space Software Co., LTD.
OnlyBelowVersion=0,7
MinVersion=0,5.0.2195
DefaultGroupName=E-Cell IDE
OutputDir=Output
VersionInfoCopyright=Copyright (C) 2005-2007 Keio University\r\nCopyright (C) 2005-2007 Mitsubishi Space Software Co., LTD.
AppVerName=E-Cell IDE alpha3
SetupIconFile=..\ide\MainWindow\MainWindow\ECellIDE.ico
BackColor2=$004080FF
BackColor=$0086ADFF
DisableProgramGroupPage=Yes
AllowNoIcons=Yes
VersionInfoDescription=E-Cell IDE
VersionInfoCompany=Keio University and Mitsubishi Space Software Co., LTD.
ArchitecturesInstallIn64BitMode=x64
ArchitecturesAllowed=x86 x64
OutputBaseFilename=E-Cell_IDE_installer
DefaultDirName={pf}\E-Cell IDE
AppName=E-Cell IDE


[Languages]
Name: "English"; MessagesFile: "C:\Program Files\Inno Setup 5\Default.isl";
Name: "Japanese"; MessagesFile: "C:\Program Files\Inno Setup 5\Languages\Japanese.isl";

[Files]
; ======================================================
; common
; ======================================================
Source: "..\bin\*.py"; DestDir: "{app}\bin";
Source: "..\sample\*"; DestDir: "{userdocs}\My E-Cell Projects\sample";
Source: "..\sample\*"; DestDir: "{commondocs}\My E-Cell Projects\sample";
Source: "..\lib\ironpython\*"; DestDir: "{app}\lib\ironpython";


; ======================================================
; bin/*.exe
; ======================================================
Source: "..\bin\MainWindow.exe"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\ipy.exe"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\ipyw.exe"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();

Source: "..\64bit\bin\MainWindow.exe"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ipy.exe"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ipyw.exe"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();


; ======================================================
; bin/*.dll
; ======================================================
Source: "..\bin\EcellCoreLibCLR.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\EcellLib.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\ecs.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\emc.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\fftpack_lite.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\fftpack_lite_core.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\Formulator.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\IronMath.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\IronPython.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\libgsl.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\libgslcblas.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\UMD.HCIL.Piccolo.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\UMD.HCIL.PiccoloX.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\WeifenLuo.WinFormsUI.Docking.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\bin\ZedGraph.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64();

Source: "..\64bit\bin\EcellCoreLibCLR.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\EcellLib.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ecs.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\emc.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\fftpack_lite.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\fftpack_lite_core.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\Formulator.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\IronMath.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\IronPython.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\libgsl.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\libgslcblas.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\UMD.HCIL.Piccolo.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\UMD.HCIL.PiccoloX.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\WeifenLuo.WinFormsUI.Docking.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ZedGraph.dll"; DestDir: "{app}\bin"; Flags: 64Bit; Check: IsWin64();


; ======================================================
; bin/ja/*
; ======================================================
Source: "..\bin\ja\AboutWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\AlignLayout.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\Analysis.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\CircularLayout.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\DistributeLayout.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\EcellLib.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\EntityListWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\GridLayout.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\MainWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\ObjectList.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\PathwayWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\PropertyWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\SearchWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\Simulation.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\StaticDebugWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\TracerWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();
Source: "..\bin\ja\ZedGraph.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 32bit; Check: not IsWin64();

Source: "..\64bit\bin\ja\AboutWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\AlignLayout.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\Analysis.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\CircularLayout.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\DistributeLayout.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\EcellLib.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\EntityListWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\GridLayout.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\MainWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\ObjectList.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\PathwayWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\PropertyWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\SearchWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\Simulation.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\StaticDebugWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\TracerWindow.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\ja\ZedGraph.resources.dll"; DestDir: "{app}\bin\ja"; Flags: 64Bit; Check: IsWin64();


; ======================================================
; bin/Microsoft.VC80.CRT/*
; ======================================================
Source: "..\bin\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest"; DestDir: "{app}\bin\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;
Source: "..\bin\Microsoft.VC80.CRT\msvcm80.dll"; DestDir: "{app}\bin\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;
Source: "..\bin\Microsoft.VC80.CRT\msvcp80.dll"; DestDir: "{app}\bin\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;
Source: "..\bin\Microsoft.VC80.CRT\msvcr80.dll"; DestDir: "{app}\bin\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;

Source: "..\bin\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;
Source: "..\bin\Microsoft.VC80.CRT\msvcm80.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;
Source: "..\bin\Microsoft.VC80.CRT\msvcp80.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;
Source: "..\bin\Microsoft.VC80.CRT\msvcr80.dll"; DestDir: "{app}\bin"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;

Source: "..\64bit\bin\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest"; DestDir: "{app}\bin\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\Microsoft.VC80.CRT\msvcm80.dll"; DestDir: "{app}\bin\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\Microsoft.VC80.CRT\msvcp80.dll"; DestDir: "{app}\bin\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\bin\Microsoft.VC80.CRT\msvcr80.dll"; DestDir: "{app}\bin\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();


; ======================================================
; plugin/*.dll
; ======================================================
Source: "..\plugin\AboutWindow.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\Analysis.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\EntityListWindow.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\MessageWindow.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\ObjectList.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\PathwayWindow.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\PropertyWindow.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\SearchWindow.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\Simulation.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\StaticDebugWindow.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\TracerWindow.dll"; DestDir: "{app}\plugin"; Flags: 32Bit; Check: not IsWin64();

Source: "..\64bit\plugin\AboutWindow.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\Analysis.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\EntityListWindow.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\MessageWindow.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\ObjectList.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\PathwayWindow.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\PropertyWindow.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\SearchWindow.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\Simulation.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\StaticDebugWindow.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\TracerWindow.dll"; DestDir: "{app}\plugin"; Flags: 64Bit; Check: IsWin64();


; ======================================================
; plugin/pathway/*
; ======================================================
Source: "..\plugin\pathway\ComponentSettings.xml"; DestDir: "{app}\plugin\pathway";

Source: "..\plugin\pathway\AlignLayout.dll"; DestDir: "{app}\plugin\pathway"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\pathway\CircularLayout.dll"; DestDir: "{app}\plugin\pathway"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\pathway\DistributeLayout.dll"; DestDir: "{app}\plugin\pathway"; Flags: 32Bit; Check: not IsWin64();
Source: "..\plugin\pathway\GridLayout.dll"; DestDir: "{app}\plugin\pathway"; Flags: 32Bit; Check: not IsWin64();

Source: "..\64bit\plugin\pathway\AlignLayout.dll"; DestDir: "{app}\plugin\pathway"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\pathway\CircularLayout.dll"; DestDir: "{app}\plugin\pathway"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\pathway\DistributeLayout.dll"; DestDir: "{app}\plugin\pathway"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\plugin\pathway\GridLayout.dll"; DestDir: "{app}\plugin\pathway"; Flags: 64Bit; Check: IsWin64();


; ======================================================
; dm/*.dll
; ======================================================
Source: "..\dm\ConstantFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\DAEStepper.dll"; DestDir: "{app}\dm"; Flags: 32Bit;  Check: not IsWin64();
Source: "..\dm\DecayFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\ESSYNSStepper.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\ExpressionAlgebraicProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\ExpressionAssignmentProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\ExpressionFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FixedDAE1Stepper.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FixedODE1Stepper.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FluxDistributionStepper.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\GillespieProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\GMAProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\MassActionFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\MichaelisUniUniFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\ODE23Stepper.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\ODE45Stepper.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\ODEStepper.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\PingPongBiBiFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\PythonProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\QuasiDynamicFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\SSystemProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\TauLeapProcess.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\TauLeapStepper.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64();

Source: "..\64bit\dm\ConstantFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\DAEStepper.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\DecayFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\ESSYNSStepper.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\ExpressionAlgebraicProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\ExpressionAssignmentProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\ExpressionFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FixedDAE1Stepper.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FixedODE1Stepper.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FluxDistributionStepper.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\GillespieProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\GMAProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\MassActionFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\MichaelisUniUniFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\ODE23Stepper.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\ODE45Stepper.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\ODEStepper.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\PingPongBiBiFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\PythonProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\QuasiDynamicFluxProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\SSystemProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\TauLeapProcess.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\TauLeapStepper.dll"; DestDir: "{app}\dm"; Flags: 64Bit; Check: IsWin64();

; ======================================================
; sample/dm/*
; ======================================================
Source: "..\dm\Algebraic1Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\Algebraic2Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\Algebraic3Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\BisectionRapidEquilibriumProcess.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\Differential1Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\Differential2Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FM1Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FM2Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FOProcess.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP01Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP02Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP03Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP11Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP12Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP13Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP14Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP21Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP22Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP23Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP24Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FP25Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FPn1Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\FPn2Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();
Source: "..\dm\MakesignalProcess.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64();

Source: "..\64bit\dm\Algebraic1Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\Algebraic2Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\Algebraic3Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\BisectionRapidEquilibriumProcess.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\Differential1Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\Differential2Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FM1Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FM2Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FOProcess.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP01Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP02Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP03Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP11Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP12Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP13Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP14Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP21Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP22Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP23Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP24Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FP25Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FPn1Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\FPn2Process.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\MakesignalProcess.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 64Bit; Check: IsWin64();


; ======================================================
; dm/Microsoft.VC80.CRT/*
; ======================================================
Source: "..\dm\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest"; DestDir: "{app}\dm\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcm80.dll"; DestDir: "{app}\dm\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcp80.dll"; DestDir: "{app}\dm\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcr80.dll"; DestDir: "{app}\dm\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;

Source: "..\dm\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcm80.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcp80.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcr80.dll"; DestDir: "{app}\dm"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;

Source: "..\64bit\dm\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest"; DestDir: "{app}\dm\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\Microsoft.VC80.CRT\msvcm80.dll"; DestDir: "{app}\dm\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\Microsoft.VC80.CRT\msvcp80.dll"; DestDir: "{app}\dm\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\Microsoft.VC80.CRT\msvcr80.dll"; DestDir: "{app}\dm\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();


; ======================================================
; sample/dm/Microsoft.VC80.CRT/*
; ======================================================
Source: "..\dm\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcm80.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcp80.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcr80.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm\Microsoft.VC80.CRT"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,5.01;

Source: "..\dm\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcm80.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcp80.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;
Source: "..\dm\Microsoft.VC80.CRT\msvcr80.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm"; Flags: 32Bit; Check: not IsWin64(); MinVersion: 0,1; OnlyBelowVersion: 0,5.01;

Source: "..\64bit\dm\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\Microsoft.VC80.CRT\msvcm80.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\Microsoft.VC80.CRT\msvcp80.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();
Source: "..\64bit\dm\Microsoft.VC80.CRT\msvcr80.dll"; DestDir: "{commondocs}\My E-Cell Projects\sample\dm\Microsoft.VC80.CRT"; Flags: 64Bit; Check: IsWin64();


; ======================================================
; tempolary
; ======================================================
Source: "gacutil.exe"; DestDir: "{app}\bin";
Source: "test.bat"; DestDir: "{app}\bin";

[Registry]
Root: HKCU; Subkey: "Environment"; ValueType: string; ValueName: "IRONPYTHONPATH"; ValueData: "%IRONPYTHONPATH%:{app}\bin";
Root: HKCU; Subkey: "Environment"; ValueType: string; ValueName: "IRONPYTHONSTARTUP"; ValueData: "{app}\lib\ironpython\__init__.py";
Root: HKCU; Subkey: "Software\KeioUniv\E-Cell IDE"; ValueType: string; ValueName: "E-Cell IDE Analysis"; ValueData: "{app}\bin";
Root: HKCU; Subkey: "Software\KeioUniv\E-Cell IDE"; ValueType: string; ValueName: "E-Cell IDE DM"; ValueData: "{app}\dm";
Root: HKCU; Subkey: "Software\KeioUniv\E-Cell IDE"; ValueType: string; ValueName: "E-Cell IDE Plugin"; ValueData: "{app}\plugin";
Root: HKCU; Subkey: "Software\KeioUniv\E-Cell IDE"; ValueType: string; ValueName: "E-Cell IDE Tmp"; ValueData: "{app}\tmp";
Root: HKCU; Subkey: "Software\KeioUniv\E-Cell IDE"; ValueType: string; ValueName: "E-Cell IDE StaticDebug Plugin"; ValueData: "{app}\plugin\staticdebug";
Root: HKLM; Subkey: "Software\KeioUniv\E-Cell IDE"; ValueType: string; ValueName: "E-Cell IDE Analysis"; ValueData: "{app}\bin";
Root: HKLM; Subkey: "Software\KeioUniv\E-Cell IDE"; ValueType: string; ValueName: "E-Cell IDE DM"; ValueData: "{app}\dm";
Root: HKLM; Subkey: "Software\KeioUniv\E-Cell IDE"; ValueType: string; ValueName: "E-Cell IDE Plugin"; ValueData: "{app}\plugin";
Root: HKLM; Subkey: "Software\KeioUniv\E-Cell IDE"; ValueType: string; ValueName: "E-Cell IDE Tmp"; ValueData: "{app}\tmp";
Root: HKLM; Subkey: "Software\KeioUniv\E-Cell IDE"; ValueType: string; ValueName: "E-Cell IDE StaticDebug Plugin"; ValueData: "{app}\plugin\staticdebug";
;Root: HKLM; Subkey: "System\CurrentControlSet\Control\SessionManager\Environment"; ValueType: string; ValueName: "IRONPYTHONPATH"; ValueData: "%IRONPYTHONPATH%:{app}\bin";
;Root: HKLM; Subkey: "System\CurrentControlSet\Control\SessionManager\Environment"; ValueType: string; ValueName: "IRONPYTHONSTARTUP"; ValueData: "{app}\lib\ironpython\__init__.py";

Root: HKCR; Subkey: ".eml"; ValueType: string; ValueName: ""; ValueData: "E-CellProgramFile"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "E-CellProgramFile"; ValueType: string; ValueName: ""; ValueData: "My Program File"; Flags: uninsdeletekey
Root: HKCR; Subkey: "E^Ce;;ProgramFile\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\bin\MainWindow.exe,0"
Root: HKCR; Subkey: "E-CellProgramFile\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\bin\MainWindow.exe"" ""%1"""

[Icons]
Name: "{group}\E-Cell IDE"; Filename: "{app}\bin\MainWindow.exe"; IconIndex: 0;
Name: "{userdesktop}\E-Cell IDE"; Filename: "{app}\bin\MainWindow.exe"; IconIndex: 0;
Name: "{group}\samples\BrabchG"; Filename: "{app}\bin\MainWindow.exe"; Parameters: "branchG.eml"; WorkingDir: "{commondocs}\My E-Cell Projects\sample\";
Name: "{group}\samples\Cascade"; Filename: "{app}\bin\MainWindow.exe"; Parameters: "cascade.eml"; WorkingDir: "{commondocs}\My E-Cell Projects\sample\";
Name: "{group}\samples\Drosophila"; Filename: "{app}\bin\MainWindow.exe"; Parameters: "Drosophila.eml"; WorkingDir: "{commondocs}\My E-Cell Projects\sample\";
Name: "{group}\samples\heatshock"; Filename: "{app}\bin\MainWindow.exe"; Parameters: "heatshock.eml"; WorkingDir: "{commondocs}\My E-Cell Projects\sample\";
Name: "{group}\samples\LTD"; Filename: "{app}\bin\MainWindow.exe"; Parameters: "LTD.eml"; WorkingDir: "{commondocs}\My E-Cell Projects\sample\";
Name: "{group}\samples\Pendulum"; Filename: "{app}\bin\MainWindow.exe"; Parameters: "Penduluma.eml"; WorkingDir: "{commondocs}\My E-Cell Projects\sample\";
Name: "{group}\samples\Simplea"; Filename: "{app}\bin\MainWindow.exe"; Parameters: "Simple.eml"; WorkingDir: "{commondocs}\My E-Cell Projects\sample\";
Name: "{group}\samples\tauleap"; Filename: "{app}\bin\MainWindow.exe"; Parameters: "tauleap.eml"; WorkingDir: "{commondocs}\My E-Cell Projects\sample\";
Name: "{group}\samples\Toy_Hybrid"; Filename: "{app}\bin\MainWindow.exe"; Parameters: "Toy_Hybrid.eml"; WorkingDir: "{commondocs}\My E-Cell Projects\sample\";

[Run]
Filename: "{app}\bin\test.bat"; Flags: runhidden; WorkingDir: "{app}\bin";

[Code]
function InitializeSetup(): Boolean;
begin
  Result := true;

  // Check for required netfx installation
  if (not RegKeyExists(HKLM, 'Software\Microsoft\.NETFramework\policy\v2.0')) then begin
    MsgBox('This application needs the Microsoft .NET Framework with version 2.0', mbInformation, MB_OK);
    Result := false
  end;
end;
