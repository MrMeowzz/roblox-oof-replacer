@echo off

color 6

echo Clearing Settings...

set settings=%cd%\assets\settings

attrib "%settings%" -R

setlocal EnableDelayedExpansion
<"!settings!" >"!settings!.~tmp" (
  for /f %%i in ('type "!settings!"^|find /c /v ""') do for /l %%j in (1 1 %%i) do (
    set "line=" &set /p "line="
    if %%j == 5 set line=roblox-shortcut-location=
    echo(!line!
    )
  )
>nul move /y "!settings!.~tmp" "!settings!"

attrib "%settings%" +R
endlocal

cls
color c
echo Successfully cleared Settings!
timeout /t 3 /nobreak >Nul