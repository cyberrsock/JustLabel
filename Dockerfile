# Используем .NET SDK 8.0 для сборки и запуска приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

# Копируем все файлы проекта
COPY . ./

WORKDIR /app/GUI

RUN dotnet build

WORKDIR /app/E2ETests

# Запуск приложения
CMD ["dotnet", "test"]
