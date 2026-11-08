@echo off
title SORA HR Platform - Mobile Phone Wi-Fi Server
echo ========================================================
echo   SORA HR Platform - Mobile Phone Wi-Fi Server
echo ========================================================
echo Starting server on local network...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0start-phone-server.ps1"
pause
