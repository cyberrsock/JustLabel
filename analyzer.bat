@echo off
setlocal enabledelayedexpansion

rem Проходим по всем файлам с расширением .cs в текущей директории и подкаталогах
for /R %%f in (*.cs) do (
    rem Получаем только имя файла с расширением
    set "filename=%%~nxf"
    
    rem Проверяем, содержит ли имя файла одно из нужных слов
    echo !filename! | findstr /i /r "Service Repository Controller Program" >nul
    if not errorlevel 1 (
        rem Запускаем main.exe с текущим файлом
        set "output="
        for /f "delims=" %%a in ('analyzer.exe "%%f"') do (
            set "output=%%a"
        )
        
        rem Выводим имя файла и результат выполнения в одной строке
        echo Processing file: !filename! - !output!
    )
)

endlocal
