# Используем .NET SDK 8.0 для сборки и запуска приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

# Копируем все файлы проекта
COPY . ./

# Запуск приложения
ENTRYPOINT ["dotnet", "test"]
