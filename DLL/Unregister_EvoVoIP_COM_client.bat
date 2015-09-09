@echo off
echo --------------------------------------------------
echo       Unregistering VoIP Evo COM control
echo --------------------------------------------------
echo Make sure you're member of the administrator group
echo otherwise the registration will fail.
regsvr32.exe /u EvoVoIP.dll
