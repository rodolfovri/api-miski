# Script para aplicar migraciones en Windows
# Ejecutar desde PowerShell

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Aplicando migraciones a PostgreSQL..." -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

# Aplicar migraciones usando dotnet ef
dotnet ef database update

Write-Host "==========================================" -ForegroundColor Green
Write-Host "? Migraciones aplicadas exitosamente" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Green
