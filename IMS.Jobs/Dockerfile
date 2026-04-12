#Stage 1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /app

COPY IMS.Jobs/*.csproj ./IMS.Jobs/

RUN dotnet restore IMS.Jobs/IMS.Jobs.csproj

COPY IMS.Jobs/ IMS.Jobs/
COPY IMS.GrpcService/Protos/Incident.proto IMS.GrpcService/Protos/

RUN dotnet publish IMS.Jobs/IMS.Jobs.csproj \
    -c ${BUILD_CONFIGURATION} \
    -o publish

#Stage 2
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "IMS.Jobs.dll"]