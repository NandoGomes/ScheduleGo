FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS sdk
WORKDIR /src
COPY . .
RUN dotnet build -c Release --force --no-cache -o /app

FROM sdk AS publish
RUN dotnet publish -c Release --no-build -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "ScheduleGo.Engine.dll"]