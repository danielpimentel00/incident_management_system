#Stage 1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /app

COPY IMS.GrpcService/*.csproj ./IMS.GrpcService/
COPY IMS.Domain/*.csproj ./IMS.Domain/
COPY IMS.Application/*.csproj ./IMS.Application/
COPY IMS.Infrastructure/*.csproj ./IMS.Infrastructure/
COPY IMS.Persistance/*.csproj ./IMS.Persistance/

RUN dotnet restore IMS.GrpcService/IMS.GrpcService.csproj

COPY . .

RUN dotnet publish IMS.GrpcService/IMS.GrpcService.csproj \
    -c ${BUILD_CONFIGURATION} \
    -o publish

#Stage 2
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

ARG HTTP_PORT=8081
ARG HTTPS_PORT=8082

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:${HTTP_PORT};https://+:${HTTPS_PORT}

EXPOSE ${HTTP_PORT}
EXPOSE ${HTTPS_PORT}

ENTRYPOINT ["dotnet", "IMS.GrpcService.dll"]