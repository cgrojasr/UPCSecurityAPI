# Multi-stage Dockerfile for UPCSecurityAPI (.NET 10)
# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies first (better layer caching)
COPY ["UPCSecurityAPI.csproj", "./"]
RUN dotnet restore "UPCSecurityAPI.csproj"

# Copy everything else and publish
COPY . .
RUN dotnet publish "UPCSecurityAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose HTTP port in Linux container
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "UPCSecurityAPI.dll"]
