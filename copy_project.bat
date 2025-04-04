@echo off
setlocal enabledelayedexpansion

:: Ask for source and destination paths
set /p source="Enter source project path: "
set /p new_name="Enter new project name: "

:: Define base path where new project folder will be created
set base_path=D:\Repo\
set folder_name="New Dotnet Project"

:: Check if the destination folder exists, if it does, increment the folder number
set counter=0
set destination=%base_path%%folder_name%

:: Loop to find a non-existing folder
:check_folder
if exist "%destination%" (
    set /a counter+=1
    set destination=%base_path%%folder_name% %counter%
    goto check_folder
)

:: Create destination directory
echo Creating new project at: %destination%
mkdir "%destination%"

:: Check if source exists
if not exist "%source%" (
    echo Error: Source directory does not exist
    pause
    exit /b 1
)

:: Copy project structure (excluding .git directory)
echo Copying files...
xcopy "%source%\*" "%destination%\" /E /H /C /I /Y

:: Remove .git directory from new project
echo Cleaning up...
rmdir "%destination%\.git" /S /Q

echo Project cloned successfully to: %destination%
pause
