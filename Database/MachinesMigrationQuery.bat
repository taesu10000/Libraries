@echo off
setlocal

set programPath=%~dp0MachinesMigrationQuery.sql
@echo %programPath%
if not exist "%programPath%" goto _DOES_NOT_EXIST
sqlcmd -S "%ComputerName%" -d "DOMINO_DB" -U "sa" -P domino -i "%programPath%"

@echo ==================== DONE ======================
goto _EXIT

:_DOES_NOT_EXIST
@echo "%programPath% Does not exist"
goto _EXIT

:_EXIT
endlocal