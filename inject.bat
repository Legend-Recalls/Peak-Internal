@echo off
echo Injecting...
smi.exe inject -p "PEAK" -a "peak.dll" -n Loading -c Loader -m Load
timeout /t 5 /NOBREAK > nul