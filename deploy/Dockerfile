# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем sln и исходники
COPY ./AccessTracker.sln ./
COPY ./source ./source
COPY ./test ./test

# Восстанавливаем зависимости
RUN dotnet restore ./AccessTracker.sln

# Публикуем основной API-проект
RUN dotnet publish ./source/AccessTracker/AccessTracker.csproj \
    -c Release -o /app/publish

# Финальный контейнер
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Переменные окружения могут быть поданы через docker-compose
ENTRYPOINT ["dotnet", "AccessTracker.dll"]
