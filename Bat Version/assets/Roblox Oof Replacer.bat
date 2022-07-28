@echo off
set settings=settings
title Roblox Oof Replacer
for /f "skip=2 delims=" %%i in (%settings%) do set "version=%%i"&goto nextline
:nextline
for /f "tokens=2 delims==" %%a in ("%version%") do (
  set version=%%a
  )
if NOT %version% == "" title Roblox Oof Replacer (Version %version%)
color 6
for /f "skip=4 delims=" %%i in (%settings%) do set "roblox_shortcut=%%i"&goto nextline
:nextline
for /f "tokens=2 delims==" %%a in ("%roblox_shortcut%") do (
  set roblox_shortcut=%%a
  )
if not exist %roblox_shortcut% (set /p roblox_shortcut=Enter the path of your Roblox Shortcut file here! (you only have to do this once or everytime you move/change your shortcut file! )
if not exist %roblox_shortcut% (exit)
attrib settings -R
setlocal EnableDelayedExpansion
<"!settings!" >"!settings!.~tmp" (
  for /f %%i in ('type "!settings!"^|find /c /v ""') do for /l %%j in (1 1 %%i) do (
    set "line=" &set /p "line="
    if %%j == 5 set line=roblox-shortcut-location=%roblox_shortcut%
    echo(!line!
    )
  )
)
>nul move /y "!settings!.~tmp" "!settings!"
attrib settings +R
endlocal

for %%i in (%roblox_shortcut%) do (
if /i "%%~xi" neq ".lnk" (
    attrib settings -R
    setlocal EnableDelayedExpansion
    <"!settings!" >"!settings!.~tmp" (
      for /f %%i in ('type "!settings!"^|find /c /v ""') do for /l %%j in (1 1 %%i) do (
        set "line=" &set /p "line="
        if %%j == 5 set line=roblox-shortcut-location=
        echo(!line!
        )
      )
    >nul move /y "!settings!.~tmp" "!settings!"
    attrib settings +R
    endlocal
)
)

set roblox_shortcut=%roblox_shortcut:"=%
for /f "delims=" %%a in ('wmic path win32_shortcutfile where "name='%roblox_shortcut:\=\\%'" get target /value') do (
    for /f "tokens=2 delims==" %%b in ("%%a") do (
    set final_location=%%b
  )
)

set final_location= %final_location%\..\content\sounds\ouch.ogg

echo DOWNLOADING OLD OOF..
powershell -c $ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest -Uri https://www.dropbox.com/s/8204ehoprc90zvf/uuhhh.ogg?dl=1 -OutFile %final_location%
cls
color c
echo SUCCESSFULLY DOWNLOADED AND REPLACED CURRENT OOF WITH OLD ONE!
timeout /t 3 /nobreak >Nul