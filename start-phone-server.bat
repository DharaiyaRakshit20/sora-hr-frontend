@echo off
title SORA HR Platform - Mobile Phone Wi-Fi Server
cd /d "%~dp0"
echo ========================================================
echo   SORA HR Platform - Mobile Phone Wi-Fi Server
echo ========================================================
if exist "phone-server.exe" (
    phone-server.exe
) else (
    powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0start-phone-server.ps1"
)
pause
