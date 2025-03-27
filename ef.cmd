@echo off
setlocal ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

set CMD=%1
set ARG1=%2
set ARG2=%3
set VERBOSE=false
set PROJECT_DIR=src\Assessments.IndigoSoft.AccessTracker.Data.Migrator

if "%CMD%"=="mg-add" (
    if "%ARG1%"=="" (
        echo Missing migration name
        exit /b 1
    )
    set MIGRATION_NAME=%ARG1%
    if not "%ARG2%"=="" set PROJECT_DIR=%ARG2%
)

if "%CMD%"=="mg-remove" if not "%ARG1%"=="" set PROJECT_DIR=%ARG1%
if "%CMD%"=="db-update" if not "%ARG1%"=="" set PROJECT_DIR=%ARG1%

if "%CMD%"=="db-revert" (
    if "%ARG1%"=="--full" (
        set MIGRATION_NAME=0
    ) else (
        set MIGRATION_NAME=%ARG1%
    )
    if not "%ARG2%"=="" set PROJECT_DIR=%ARG2%
)

pushd %PROJECT_DIR%

if "%CMD%"=="mg-add" (
    echo 📦 Adding migration %MIGRATION_NAME%...
    dotnet ef migrations add %MIGRATION_NAME%
    echo ✅ Migration added.
)

if "%CMD%"=="mg-remove" (
    echo ❌ Removing last migration...
    dotnet ef migrations remove -f
    echo ✅ Migration removed.
)

if "%CMD%"=="db-update" (
    echo ⏳ Updating database...
    dotnet ef database update
    echo ✅ Database updated.
)

if "%CMD%"=="db-revert" (
    if "%MIGRATION_NAME%"=="" (
        for /f "tokens=1*" %%i in ('dotnet ef migrations list ^| tail -n 2') do (
            set MIGRATION_NAME=%%i
            goto found
        )
        :found
    )
    if "%MIGRATION_NAME%"=="" (
        echo ❌ No previous migration found.
    ) else (
        echo ↩️ Reverting to: %MIGRATION_NAME%
        dotnet ef database update %MIGRATION_NAME%
        echo ✅ Revert complete.
    )
)

popd
