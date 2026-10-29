@echo off
rem Starts the web app and opens it in the browser. Works from any folder.
rem Press Ctrl+C in this window to stop it.
dotnet watch --project "%~dp0src\VolvoHackathon"
pause
