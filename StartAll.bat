@echo off
rem Starts the API server and the web app, each in its own window.
rem Close a window (or press Ctrl+C in it) to stop that app.
start "Volvo API server" "%~dp0volvo-hackathon-2026-api-server\Start.bat"
start "Volvo web app" "%~dp0volvo-hackathon-2026\Start.bat"
