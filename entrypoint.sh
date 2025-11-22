#!/bin/bash
set -e

echo "=========================================="
echo "Miski API - Iniciando..."
echo "=========================================="

echo "Esperando a que PostgreSQL esté listo..."

# Esperar hasta que PostgreSQL esté disponible
until nc -z postgres 5432 2>/dev/null; do
  echo "PostgreSQL no está listo, esperando 2 segundos..."
  sleep 2
done

echo "=========================================="
echo "PostgreSQL listo"
echo "Iniciando aplicación Miski.Api..."
echo "=========================================="

exec dotnet Miski.Api.dll