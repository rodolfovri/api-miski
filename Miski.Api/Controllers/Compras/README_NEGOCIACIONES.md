# M�dulo de Compras - Negociaciones

Este m�dulo gestiona todo el proceso de negociaciones de compra de productos.

## Funcionalidades principales:
- CRUD completo de negociaciones
- Subida y almacenamiento de fotos (calidad producto, DNI frontal y posterior)
- Aprobaci�n/rechazo de negociaciones
- Filtrado avanzado por proveedor, comisionista, producto, estado
- C�lculo autom�tico de monto total

## Controladores:
- `NegociacionesController`: CRUD completo con manejo de archivos

## Estructura del m�dulo:

### Controllers (API Layer)
```
Miski.Api/Controllers/Compras/
??? NegociacionesController.cs
```

### Features (Application Layer)
```
Miski.Application/Features/Compras/Negociaciones/
??? Commands/
?   ??? CreateNegociacion/
?   ?   ??? CreateNegociacionCommand.cs
?   ?   ??? CreateNegociacionHandler.cs
?   ?   ??? CreateNegociacionValidator.cs
?   ??? UpdateNegociacion/
?   ?   ??? UpdateNegociacionCommand.cs
?   ?   ??? UpdateNegociacionHandler.cs
?   ??? DeleteNegociacion/
?   ?   ??? DeleteNegociacionCommand.cs
?   ?   ??? DeleteNegociacionHandler.cs
?   ??? AprobarNegociacion/
?       ??? AprobarNegociacionCommand.cs
?       ??? AprobarNegociacionHandler.cs
??? Queries/
    ??? GetNegociaciones/
    ?   ??? GetNegociacionesQuery.cs
    ?   ??? GetNegociacionesHandler.cs
    ??? GetNegociacionById/
        ??? GetNegociacionByIdQuery.cs
        ??? GetNegociacionByIdHandler.cs
```

### Services (Application Layer)
```
Miski.Application/Services/
??? FileStorageService.cs
    ??? IFileStorageService (Interface)
    ??? LocalFileStorageService (Implementaci�n local)
    ??? CloudFileStorageService (Comentado - Para futuro)
```

### DTOs (Shared Layer)
```
Miski.Shared/DTOs/Compras/
??? NegociacionDto.cs
    ??? NegociacionDto
    ??? CreateNegociacionDto
    ??? UpdateNegociacionDto
    ??? AprobarNegociacionDto
```

## Endpoints Disponibles:

### 1. Obtener todas las negociaciones
```http
GET /api/compras/negociaciones
Query params: 
  ?idProveedor=1
  &idComisionista=2
  &idProducto=3
  &estadoAprobado=PENDIENTE
  &estado=ACTIVO
```

### 2. Obtener negociaci�n por ID
```http
GET /api/compras/negociaciones/{id}
```

### 3. Crear negociaci�n
```http
POST /api/compras/negociaciones
Content-Type: multipart/form-data

FormData:
{
  "idProveedor": 1,
  "idComisionista": 2,
  "idProducto": 3,
  "pesoTotal": 1500.50,
  "sacosTotales": 60,
  "precioUnitario": 8.50,
  "nroCuentaRuc": "12345678901",
  "fotoCalidadProducto": [archivo],
  "fotoDniFrontal": [archivo],
  "fotoDniPosterior": [archivo],
  "observacion": "Producto de alta calidad",
  "estado": "ACTIVO"
}
```

### 4. Actualizar negociaci�n
```http
PUT /api/compras/negociaciones/{id}
Content-Type: multipart/form-data

FormData:
{
  "idNegociacion": 1,
  "idProveedor": 1,
  "idComisionista": 2,
  "idProducto": 3,
  "pesoTotal": 1600.00,
  "sacosTotales": 65,
  "precioUnitario": 8.75,
  "nroCuentaRuc": "12345678901",
  "fotoCalidadProducto": [archivo opcional],
  "fotoDniFrontal": [archivo opcional],
  "fotoDniPosterior": [archivo opcional],
  "observacion": "Actualizaci�n",
  "estado": "ACTIVO"
}
```

### 5. Eliminar (inactivar) negociaci�n
```http
DELETE /api/compras/negociaciones/{id}
```

### 6. Obtener negociaciones pendientes
```http
GET /api/compras/negociaciones/pendientes-aprobacion
```

### 7. Aprobar negociaci�n
```http
PUT /api/compras/negociaciones/{id}/aprobar
Content-Type: application/json

{
  "idNegociacion": 1,
  "aprobadaPor": 5,
  "observacion": "Aprobado por buena calidad"
}
```

## Validaciones Implementadas:

### CreateNegociacion:
- ? Comisionista debe existir y ser v�lido
- ? Proveedor debe existir (si se proporciona)
- ? Producto debe existir (si se proporciona)
- ? Peso total: 0 < peso ? 50,000 kg
- ? Sacos totales: 0 < sacos ? 2,000 unidades
- ? Precio unitario: 0 < precio ? 1,000 soles/kg
- ? NroCuentaRuc: longitud entre 8-20 caracteres
- ? Las tres fotos son obligatorias
- ? Coherencia peso/sacos: promedio 20-30 kg por saco
- ? Observaciones: m�ximo 500 caracteres

### UpdateNegociacion:
- ? Negociaci�n debe existir
- ? No se puede actualizar negociaci�n aprobada
- ? Validaciones de entidades relacionadas
- ? Fotos opcionales (solo actualiza si se env�an nuevas)
- ? Elimina fotos antiguas al actualizar

### DeleteNegociacion:
- ? Negociaci�n debe existir
- ? No puede tener compras asociadas
- ? Elimina las fotos del almacenamiento
- ? Soft delete (estado INACTIVO)

### AprobarNegociacion:
- ? Negociaci�n debe existir
- ? Debe estar en estado PENDIENTE
- ? Usuario aprobador debe existir
- ? Registra fecha de aprobaci�n autom�ticamente

## Almacenamiento de Archivos:

### Configuraci�n Local:
```
Ruta base: C:\MiskiFiles\
Estructura:
  ??? negociaciones/
  ?   ??? calidad/
  ?   ?   ??? {guid}.{ext}
  ?   ??? dni/
  ?       ??? {guid}.{ext} (frontal)
  ?       ??? {guid}.{ext} (posterior)
```

### Nombres de archivo:
- Formato: `{Guid}.{extensi�n}`
- Ejemplo: `a1b2c3d4-e5f6-7890-abcd-ef1234567890.jpg`

### URLs retornadas:
- `/negociaciones/calidad/{guid}.{ext}`
- `/negociaciones/dni/{guid}.{ext}`

### Configuraci�n Futura (Servidor en la nube):
```csharp
// CloudFileStorageService implementar�:
// - Azure Blob Storage
// - AWS S3
// - Google Cloud Storage
```

## Entidades Relacionadas:
- **Negociacion** ? Entidad principal
- **Persona (Proveedor)** ? Persona que provee el producto
- **Persona (Comisionista)** ? Persona que gestiona la negociaci�n
- **Persona (Aprobador)** ? Persona que aprueba la negociaci�n
- **Producto** ? Producto negociado
- **Compra** ? Compras generadas de la negociaci�n

## Arquitectura Aplicada:
- ? Clean Architecture
- ? CQRS Pattern (Commands y Queries separadas)
- ? MediatR para desacoplar handlers
- ? Repository Pattern + Unit of Work
- ? AutoMapper para mapeo de DTOs
- ? FluentValidation para validaciones robustas
- ? File Storage Service (abstracci�n para almacenamiento)
- ? Manejo de excepciones personalizado

## Estados de Negociaci�n:

### EstadoAprobado:
- `PENDIENTE`: Esperando aprobaci�n
- `APROBADO`: Negociaci�n aprobada
- `RECHAZADO`: Negociaci�n rechazada

### Estado General:
- `ACTIVO`: Negociaci�n activa
- `INACTIVO`: Negociaci�n eliminada (soft delete)

## Flujo de Negociaci�n:

1. **Creaci�n**: Comisionista crea negociaci�n con fotos ? Estado: PENDIENTE
2. **Revisi�n**: Supervisor revisa negociaci�n pendiente
3. **Aprobaci�n**: Supervisor aprueba ? Estado: APROBADO
4. **Compra**: Se pueden crear compras basadas en negociaci�n aprobada

## C�lculos Autom�ticos:

### MontoTotal:
```csharp
MontoTotal = PesoTotal � PrecioUnitario
```

### Peso Promedio por Saco:
```csharp
PesoPromedio = PesoTotal � SacosTotales
// Debe estar entre 20-30 kg
```

## Prioridad: ALTA
Las negociaciones son el punto de partida del proceso de compras.
