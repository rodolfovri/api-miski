# M�dulo de Ubicaciones

Este m�dulo gestiona ubicaciones fijas y tracking en tiempo real.

## Funcionalidades principales:
- CRUD de ubicaciones fijas (plantas, centros de acopio)
- Tracking en tiempo real de comisionistas
- Historial de ubicaciones
- Monitoreo de rutas

## Controladores:
- `UbicacionesController`: CRUD de ubicaciones fijas
- `TrackingController`: Tracking en tiempo real

## Casos de uso por implementar:
- Crear/actualizar/eliminar ubicaciones
- Registrar posiciones de tracking
- Consultar �ltima ubicaci�n conocida
- Obtener historial de ubicaciones
- Monitorear comisionistas activos

## Entidades relacionadas:
- `Ubicacion`
- `PersonaUbicacion`
- `TrackingPersona`

## Funcionalidades avanzadas:
- Geofencing
- Alertas de ubicaci�n
- Optimizaci�n de rutas