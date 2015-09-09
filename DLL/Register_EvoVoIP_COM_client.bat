@echo off
echo --------------------------------------------------
echo        Registering VoIP Evo COM control
echo --------------------------------------------------
echo Make sure you're member of the administrator group
echo otherwise the registration will fail.
regsvr32.exe EvoVoIP.dll
