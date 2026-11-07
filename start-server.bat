@echo off
title SORA HR Platform - Local Server
echo ========================================================
echo   SORA HR Platform - Local Development Server
echo ========================================================
echo.
echo Starting server at http://localhost:5173 ...
echo Public Landing Page: http://localhost:5173/
echo Admin Portal:       http://localhost:5173/admin/
echo.
start http://localhost:5173
python -m http.server 5173 --bind 127.0.0.1
if %ERRORLEVEL% NEQ 0 (
    echo Python standard command failed, trying Python launcher...
    py -m http.server 5173 --bind 127.0.0.1
)
pause
