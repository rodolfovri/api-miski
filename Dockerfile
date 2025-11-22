# Etapa 1: Build - Imagen con SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar el archivo del proyecto y restaurar dependencias
COPY ["Miski.Api.csproj", "./"]
RUN dotnet restore "Miski.Api.csproj"

# Copiar todo el código fuente y compilar
COPY . .
RUN dotnet build "Miski.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa 2: Publish - Publicar la aplicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Miski.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 3: Final - Imagen final con la aplicación publicada
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instalar netcat para verificar conectividad con PostgreSQL
RUN apt-get update && apt-get install -y \
    netcat-openbsd \
    && rm -rf /var/lib/apt/lists/*

# Copiar la aplicación publicada
COPY --from=publish /app/publish .

# Crear directorio para archivos subidos
RUN mkdir -p /app/uploads && chmod 777 /app/uploads

# Copiar el script de entrada
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

EXPOSE 8080
EXPOSE 8081

# Usar el script de entrada
ENTRYPOINT ["./entrypoint.sh"]