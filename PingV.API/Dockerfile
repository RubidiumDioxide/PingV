# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /src

# Copy project file and restore dependencies
COPY ["PingV.API/PingV.API.csproj", "PingV.API/"]
COPY ["PingV.Domain/PingV.Domain.csproj", "PingV.Domain/"]
COPY ["PingV.Application/PingV.Application.csproj", "PingV.Application/"]
COPY ["PingV.Infrastructure/PingV.Infrastructure.csproj", "PingV.Infrastructure/"]
COPY ["PingV.Infrastructure.PostgreSQL/PingV.Infrastructure.PostgreSQL.csproj", "PingV.Infrastructure.PostgreSQL/"]
COPY ["PingV.Infrastructure.SQLServer/PingV.Infrastructure.SQLServer.csproj", "PingV.Infrastructure.SQLServer/"]

RUN dotnet restore "PingV.API/PingV.API.csproj"

# Copy remaining source code and build
COPY . .

WORKDIR /src/PingV.API
RUN dotnet publish "PingV.API.csproj" -c Release -o /app/publish /p:UseAppHost=false 

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080

COPY --from=builder /app/publish .
ENTRYPOINT ["dotnet", "PingV.API.dll"] 
