# Stage de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# ==============================================================================
# 1. COPIAR TODOS OS PROJETOS (.CSPROJ) DAS CAMADAS
# ==============================================================================
# Isso garante que o 'dotnet restore' encontre as dependências da Clean Architecture
COPY ["src/4Manager.API/4Tech.4Manager.API.csproj", "src/4Manager.API/"]
COPY ["src/4Manager.Application/4Tech.4Manager.Application.csproj", "src/4Manager.Application/"]
COPY ["src/4Manager.Domain/4Tech.4Manager.Domain.csproj", "src/4Manager.Domain/"]
COPY ["src/4Manager.Infrastructure/4Tech.4Manager.Infrastructure.csproj", "src/4Manager.Infrastructure/"]
COPY ["src/4Manager.Persistence/4Tech.4Manager.Persistence.csproj", "src/4Manager.Persistence/"]

# 2. Restaurar dependências
RUN dotnet restore "src/4Manager.API/4Tech.4Manager.API.csproj"

# 3. Copiar todo o código fonte restante
COPY . .

# 4. Build e Publish
WORKDIR "/src/src/4Manager.API"
RUN dotnet publish "4Tech.4Manager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage Final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "4Tech.4Manager.API.dll"]