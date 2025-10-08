# Estructura Organizacional del Proyecto Miski

## M�dulos de Negocio

El proyecto est� organizado en los siguientes m�dulos principales:

### 1. Auth (Autenticaci�n)
- **Ruta base**: `/api/auth`
- **Tag Swagger**: `Auth`
- **Controladores**: `AuthController`
- **Responsabilidades**: Login, registro, gesti�n de perfiles, JWT

### 2. Compras
- **Ruta base**: `/api/compras`
- **Tag Swagger**: `Compras`
- **Controladores**: 
  - `NegociacionesController` (`/api/compras/negociaciones`)
  - `ComprasController` (`/api/compras`)
  - `LotesController` (`/api/compras/lotes`)
  - `LlegadaPlantaController` (`/api/compras/llegadas-planta`)
- **Responsabilidades**: Gesti�n completa del proceso de compras

### 3. Maestros
- **Ruta base**: `/api/maestros`
- **Tag Swagger**: `Maestros`
- **Controladores**: 
  - `ProductosController` (`/api/maestros/productos`)
  - `TipoDocumentoController` (`/api/maestros/tipo-documento`)
  - `VehiculosController` (`/api/maestros/vehiculos`)
- **Responsabilidades**: Gesti�n de tablas maestras
- **Prioridad**: ALTA - Son fundamentales para el sistema

### 4. Personas
- **Ruta base**: `/api/personas`
- **Tag Swagger**: `Personas`
- **Controladores**: 
  - `PersonasController` (`/api/personas`)
  - `CategoriaPersonaController` (`/api/personas/categorias`)
- **Responsabilidades**: Gesti�n de personas, usuarios, categor�as

### 5. Ubicaciones
- **Ruta base**: `/api/ubicaciones`
- **Tag Swagger**: `Ubicaciones`
- **Controladores**: 
  - `UbicacionesController` (`/api/ubicaciones`)
  - `TrackingController` (`/api/ubicaciones/tracking`)
- **Responsabilidades**: Ubicaciones fijas y tracking en tiempo real

## Estructura de Carpetas

```
Miski.Api/
??? Controllers/
?   ??? Auth/
?   ?   ??? AuthController.cs
?   ?   ??? README.md
?   ??? Compras/
?   ?   ??? ComprasController.cs
?   ?   ??? NegociacionesController.cs
?   ?   ??? LotesController.cs
?   ?   ??? LlegadaPlantaController.cs
?   ?   ??? README.md
?   ??? Maestros/
?   ?   ??? ProductosController.cs
?   ?   ??? TipoDocumentoController.cs
?   ?   ??? VehiculosController.cs
?   ?   ??? README.md
?   ??? Personas/
?   ?   ??? PersonasController.cs
?   ?   ??? CategoriaPersonaController.cs
?   ?   ??? README.md
?   ??? Ubicaciones/
?       ??? UbicacionesController.cs
?       ??? TrackingController.cs
?       ??? README.md
??? wwwroot/
?   ??? swagger/
?       ??? custom.css
??? ...
```

## Application Layer - Features

```
Miski.Application/
??? Features/
    ??? Auth/
    ?   ??? Commands/
    ?   ??? Queries/
    ??? Compras/
    ?   ??? Negociaciones/
    ?   ??? Compras/
    ?   ??? Lotes/
    ?   ??? LlegadaPlanta/
    ??? Maestros/
    ?   ??? Productos/
    ?   ??? TipoDocumento/
    ?   ??? Vehiculos/
    ??? Personas/
    ?   ??? Personas/
    ?   ??? Categorias/
    ??? Ubicaciones/
        ??? Ubicaciones/
        ??? Tracking/
```

## Orden de Implementaci�n

### Fase 1 - Tablas Maestras (Prioridad Alta)
1. TipoDocumento CRUD
2. Productos CRUD
3. Veh�culos CRUD
4. CategoriaPersona CRUD

### Fase 2 - Personas y Usuarios
1. Personas CRUD
2. Gesti�n de categor�as por persona
3. Usuarios y roles

### Fase 3 - Compras B�sicas
1. Negociaciones CRUD
2. Compras CRUD
3. Lotes CRUD

### Fase 4 - Procesos Avanzados
1. LlegadaPlanta
2. Tracking en tiempo real
3. Reportes y dashboards

## Configuraci�n de Swagger

- Los controladores aparecen agrupados por m�dulo usando el atributo `[Tags("NombreModulo")]`
- Cada m�dulo tiene un color distintivo en la interfaz
- Documentaci�n enriquecida con comentarios XML
- Configuraci�n de JWT Bearer para autenticaci�n