# Базовый ASP.NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8070
EXPOSE 8071

# SDK + Node.js для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Установим Node.js
RUN apt-get update && \
    apt-get install -y curl && \
    curl -sL https://deb.nodesource.com/setup_20.x | bash && \
    apt-get install -y nodejs

# Копируем csproj и esproj
COPY ["OneHostReactApp.Server/OneHostReactApp.Server.csproj", "OneHostReactApp.Server/"]
COPY ["onehostreactapp.client/onehostreactapp.client.esproj", "onehostreactapp.client/"]

# Восстановление пакетов
RUN dotnet restore "OneHostReactApp.Server/OneHostReactApp.Server.csproj"

# Копируем все файлы
COPY . .

# Сборка фронта вручную
WORKDIR /src/onehostreactapp.client
RUN npm install
RUN npm run build

# Копируем фронт в wwwroot
RUN rm -rf /src/OneHostReactApp.Server/wwwroot/*
RUN mkdir -p /src/OneHostReactApp.Server/wwwroot
RUN cp -r dist/* /src/OneHostReactApp.Server/wwwroot/

# Сборка бэкенда
WORKDIR /src/OneHostReactApp.Server
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OneHostReactApp.Server.dll"]