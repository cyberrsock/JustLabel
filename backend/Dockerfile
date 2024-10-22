# Используем .NET SDK 8.0 для сборки и запуска приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

# Настраиваем переменные среды для использования сертификата
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="123"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

# Копируем все файлы проекта
COPY . ./

RUN dotnet restore

# Генерация разработческого сертификата внутри контейнера
# RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p "123"

# Запуск приложения
CMD ["dotnet", "test"]
