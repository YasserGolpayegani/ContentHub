# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["ContentHub/ContentHub.Api.csproj", "ContentHub/"]
COPY ["ContentHub.Application/ContentHub.Application.csproj", "ContentHub.Application/"]
COPY ["ContentHub.Domain/ContentHub.Domain.csproj", "ContentHub.Domain/"]
COPY ["ContentHub.Infrastructure/ContentHub.Infrastructure.csproj", "ContentHub.Infrastructure/"]
COPY ["ContentHub.Persistence/ContentHub.Persistence.csproj", "ContentHub.Persistence/"]

RUN dotnet restore "ContentHub/ContentHub.Api.csproj"

COPY . .

WORKDIR "/src/ContentHub"

RUN dotnet publish "ContentHub.Api.csproj" \
    -c Release \
    -o /app/publish \
    --self-contained false \
    -p:UseAppHost=false \
    -p:GenerateRuntimeConfigurationFiles=true


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ContentHub.Api.dll"]