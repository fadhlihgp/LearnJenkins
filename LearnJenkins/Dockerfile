FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Salin semua file sebelum restore
COPY . .  

# Tentukan file solusi atau proyek yang akan direstore
RUN dotnet restore LearnJenkins.csproj  # Gantilah sesuai nama file `.sln` atau `.csproj`

# Build dan publish
RUN dotnet publish -c Release -o out /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 80
ENTRYPOINT ["dotnet", "LearnJenkins.dll"]
