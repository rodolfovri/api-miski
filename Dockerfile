# Etapa 1: Build - Imagen con SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Configurar locale UTF-8
ENV LANG=C.UTF-8
ENV LC_ALL=C.UTF-8

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar el archivo del proyecto y restaurar dependencias
COPY ["Miski.Api.csproj", "./"]
RUN dotnet restore "Miski.Api.csproj"

# Copiar todo el codigo fuente y compilar
COPY . .
RUN dotnet build "Miski.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa 2: Publish - Publicar la aplicacion
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Miski.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 3: Final - Imagen final con la aplicacion publicada
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Configurar locale UTF-8 en runtime
ENV LANG=C.UTF-8
ENV LC_ALL=C.UTF-8
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app

# Instalar netcat para verificar conectividad con PostgreSQL
RUN apt-get update && apt-get install -y \
    netcat-openbsd \
    locales \
    && echo "es_ES.UTF-8 UTF-8" > /etc/locale.gen \
    && locale-gen \
    && rm -rf /var/lib/apt/lists/*

# Copiar la aplicacion publicada
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
