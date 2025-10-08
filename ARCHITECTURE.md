# Estructura Organizacional del Proyecto Miski

## Módulos de Negocio

El proyecto está organizado en los siguientes módulos principales:

### 1. Auth (Autenticación)
- **Ruta base**: `/api/auth`
- **Tag Swagger**: `Auth`
- **Controladores**: `AuthController`
- **Responsabilidades**: Login, registro, gestión de perfiles, JWT

### 2. Compras
- **Ruta base**: `/api/compras`
- **Tag Swagger**: `Compras`
- **Controladores**: 
  - `NegociacionesController` (`/api/compras/negociaciones`)
  - `ComprasController` (`/api/compras`)
  - `LotesController` (`/api/compras/lotes`)
  - `LlegadaPlantaController` (`/api/compras/llegadas-planta`)
- **Responsabilidades**: Gestión completa del proceso de compras

### 3. Maestros
- **Ruta base**: `/api/maestros`
- **Tag Swagger**: `Maestros`
- **Controladores**: 
  - `ProductosController` (`/api/maestros/productos`)
  - `TipoDocumentoController` (`/api/maestros/tipo-documento`)
  - `VehiculosController` (`/api/maestros/vehiculos`)
- **Responsabilidades**: Gestión de tablas maestras
- **Prioridad**: ALTA - Son fundamentales para el sistema

### 4. Personas
- **Ruta base**: `/api/personas`
- **Tag Swagger**: `Personas`
- **Controladores**: 
  - `PersonasController` (`/api/personas`)
  - `CategoriaPersonaController` (`/api/personas/categorias`)
- **Responsabilidades**: Gestión de personas, usuarios, categorías

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

## Orden de Implementación

### Fase 1 - Tablas Maestras (Prioridad Alta)
1. TipoDocumento CRUD
2. Productos CRUD
3. Vehículos CRUD
4. CategoriaPersona CRUD

### Fase 2 - Personas y Usuarios
1. Personas CRUD
2. Gestión de categorías por persona
3. Usuarios y roles

### Fase 3 - Compras Básicas
1. Negociaciones CRUD
2. Compras CRUD
3. Lotes CRUD

### Fase 4 - Procesos Avanzados
1. LlegadaPlanta
2. Tracking en tiempo real
3. Reportes y dashboards

## Configuración de Swagger

- Los controladores aparecen agrupados por módulo usando el atributo `[Tags("NombreModulo")]`
- Cada módulo tiene un color distintivo en la interfaz
- Documentación enriquecida con comentarios XML
- Configuración de JWT Bearer para autenticación