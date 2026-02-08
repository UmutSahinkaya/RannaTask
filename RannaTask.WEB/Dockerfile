# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["RannaTask.WEB/RannaTask.WEB.csproj", "RannaTask.WEB/"]
RUN dotnet restore "RannaTask.WEB/RannaTask.WEB.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/RannaTask.WEB"
RUN dotnet build "RannaTask.WEB.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "RannaTask.WEB.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RannaTask.WEB.dll"]
