#Stage 1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /app

COPY incident_management_system.API/*.csproj ./incident_management_system.API/
COPY IMS.Domain/*.csproj ./IMS.Domain/
COPY IMS.Application/*.csproj ./IMS.Application/
COPY IMS.Infrastructure/*.csproj ./IMS.Infrastructure/
COPY IMS.Persistance/*.csproj ./IMS.Persistance/

RUN dotnet restore incident_management_system.API/incident_management_system.API.csproj

COPY . .

RUN dotnet publish incident_management_system.API/incident_management_system.API.csproj \
    -c ${BUILD_CONFIGURATION} \
    -o publish

#Stage 2
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

ARG APP_PORT=8080

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:${APP_PORT}

EXPOSE ${APP_PORT}

ENTRYPOINT ["dotnet", "incident_management_system.API.dll"]