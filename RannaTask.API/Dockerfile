# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["RannaTask.API/RannaTask.API.csproj", "RannaTask.API/"]
COPY ["RannaTask.Business/RannaTask.Business.csproj", "RannaTask.Business/"]
COPY ["RannaTask.DAL/RannaTask.DAL.csproj", "RannaTask.DAL/"]
COPY ["RannaTask.Entities/RannaTask.Entities.csproj", "RannaTask.Entities/"]

RUN dotnet restore "RannaTask.API/RannaTask.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/RannaTask.API"
RUN dotnet build "RannaTask.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "RannaTask.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RannaTask.API.dll"]
