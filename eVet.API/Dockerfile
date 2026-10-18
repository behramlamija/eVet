FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eVet.API/eVet.API.csproj", "eVet.API/"]
COPY ["eVet.Services/eVet.Services.csproj", "eVet.Services/"]
COPY ["eVet.Model/eVet.Model.csproj", "eVet.Model/"]
RUN dotnet restore "./eVet.API/eVet.API.csproj"
COPY . .
WORKDIR "/src/eVet.API"
RUN dotnet publish "./eVet.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "eVet.API.dll"]
