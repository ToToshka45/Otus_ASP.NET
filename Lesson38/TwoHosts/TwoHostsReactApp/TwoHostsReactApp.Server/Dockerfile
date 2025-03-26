FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8090

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TwoHostsReactApp.Server/TwoHostsReactApp.Server.csproj", "TwoHostsReactApp.Server/"]
RUN dotnet restore "TwoHostsReactApp.Server/TwoHostsReactApp.Server.csproj"
COPY . .
WORKDIR "/src/TwoHostsReactApp.Server"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TwoHostsReactApp.Server.dll"]