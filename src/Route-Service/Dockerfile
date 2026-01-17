FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /repo

COPY . .
RUN dotnet publish ./src/Route-Service/Route-Service.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Route-Service.dll"]
