#!/bin/bash
# Script para aplicar migraciones manualmente desde tu máquina local

echo "=========================================="
echo "Aplicando migraciones a PostgreSQL..."
echo "=========================================="

# Aplicar migraciones usando dotnet ef desde tu máquina local
dotnet ef database update

echo "=========================================="
echo "? Migraciones aplicadas exitosamente"
echo "=========================================="
