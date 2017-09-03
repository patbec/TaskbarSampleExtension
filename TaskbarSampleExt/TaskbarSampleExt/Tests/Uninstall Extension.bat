@echo off

title Shell Extension: Deinstallation

echo ------------------------------------------------------------------------------------------------------------------------
echo ----------------------------------------------- Deinstallation ausfhren -----------------------------------------------
echo ------------------------------------------------------------------------------------------------------------------------
echo.

SET path_dll=TaskbarSampleExt.dll
SET path_gacutil=%PROGRAMFILES(X86)%\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\x64\gacutil.exe
SET path_regasm=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe

net session >nul 2>&1
if NOT %errorLevel% == 0 (
	echo Bitte als Administrator starten
	pause >nul
	exit
)

pause

cd %~dp0\..\bin\Debug

if Not exist "%path_gacutil%" (
    echo Die Datei gacutil wurde nicht gefunden, passen Sie den Pfad in dieser Datei an.
	pause >nul
	exit
)

if NOT exist "%path_regasm%" (
    echo Die Datei regasm wurde nicht gefunden, passen Sie den Pfad in dieser Datei an.
	pause >nul
	exit
)

if NOT exist %path_dll% (
    echo Die Datei %path_dll% wurde nicht gefunden, prfen Sie das Verzeichnis.
	pause >nul
	exit
)

"%path_gacutil%" /u "TaskbarSampleExt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=67cc9ccd325125f6"
"%path_regasm%" /u %path_dll%

echo.
echo ------------------------------------------------------------------------------------------------------------------------
echo --------------------------------------------- Deinstallation abgeschlossen ---------------------------------------------
echo ------------------------------------------------------------------------------------------------------------------------
echo 
echo Drcken Sie eine beliebige Taste um den Explorer neu zu starten.
echo Dies ist nur n”tig wenn die Erweiterung vor der Deinstallation nicht deaktiviert wurde.

pause >nul

taskkill.exe /im explorer.exe /f
explorer