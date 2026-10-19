FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eVet.Worker/eVet.Worker.csproj", "eVet.Worker/"]
COPY ["eVet.Messaging/eVet.Messaging.csproj", "eVet.Messaging/"]
RUN dotnet restore "./eVet.Worker/eVet.Worker.csproj"
COPY . .
WORKDIR "/src/eVet.Worker"
RUN dotnet publish "./eVet.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "eVet.Worker.dll"]
