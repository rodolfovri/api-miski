# M�dulo de Compras - Asignaci�n de Veh�culos

Este m�dulo gestiona la asignaci�n de compras a veh�culos con gu�as de remisi�n.

## Funcionalidades principales:
- Asignar m�ltiples compras a un veh�culo
- Registro de gu�a de remisi�n
- Consulta de asignaciones con filtros
- Validaci�n de compras no duplicadas

## Controladores:
- `CompraVehiculosController`: CRUD de asignaciones de compras a veh�culos

## Estructura del m�dulo:

### Controllers (API Layer)
```
Miski.Api/Controllers/Compras/
??? CompraVehiculosController.cs
```

### Features (Application Layer)
```
Miski.Application/Features/Compras/CompraVehiculos/
??? Commands/
?   ??? CreateCompraVehiculo/
?       ??? CreateCompraVehiculoCommand.cs
?       ??? CreateCompraVehiculoHandler.cs
?       ??? CreateCompraVehiculoValidator.cs
??? Queries/
    ??? GetCompraVehiculos/
    ?   ??? GetCompraVehiculosQuery.cs
    ?   ??? GetCompraVehiculosHandler.cs
    ??? GetCompraVehiculoById/
        ??? GetCompraVehiculoByIdQuery.cs
        ??? GetCompraVehiculoByIdHandler.cs
```

### DTOs (Shared Layer)
```
Miski.Shared/DTOs/Compras/
??? CompraVehiculoDto.cs
    ??? CompraVehiculoDto
    ??? CompraVehiculoDetalleDto
    ??? CreateCompraVehiculoDto
    ??? UpdateCompraVehiculoDto
```

## Endpoints Disponibles:

### 1. Obtener todas las asignaciones de compras a veh�culos
```http
GET /api/compras/asignacion-vehiculos
Query params: 
  ?idVehiculo=1
  &guiaRemision=GR-2024
```

**Response:**
```json
{
  "success": true,
  "message": "Asignaciones de compras a veh�culos obtenidas exitosamente",
  "data": [
    {
      "idCompraVehiculo": 1,
      "idVehiculo": 1,
      "guiaRemision": "GR-2024-001",
      "fRegistro": "2024-01-15T10:30:00",
      "vehiculoPlaca": "ABC-123",
      "vehiculoMarca": "Volvo",
      "vehiculoModelo": "FH 500",
      "detalles": [
        {
          "idCompraVehiculoDetalle": 1,
          "idCompraVehiculo": 1,
          "idCompra": 5,
          "compraSerie": "C-2024-005",
          "compraFRegistro": "2024-01-10T08:00:00",
          "compraMontoTotal": 85000.00,
          "compraNegociacionId": "3"
        },
        {
          "idCompraVehiculoDetalle": 2,
          "idCompraVehiculo": 1,
          "idCompra": 6,
          "compraSerie": "C-2024-006",
          "compraFRegistro": "2024-01-12T09:30:00",
          "compraMontoTotal": 112500.00,
          "compraNegociacionId": "4"
        }
      ]
    }
  ]
}
```

### 2. Obtener asignaci�n por ID
```http
GET /api/compras/asignacion-vehiculos/{id}
```

**Response:**
```json
{
  "success": true,
  "message": "Asignaci�n de compra a veh�culo obtenida exitosamente",
  "data": {
    "idCompraVehiculo": 1,
    "idVehiculo": 1,
    "guiaRemision": "GR-2024-001",
    "fRegistro": "2024-01-15T10:30:00",
    "vehiculoPlaca": "ABC-123",
    "vehiculoMarca": "Volvo",
    "vehiculoModelo": "FH 500",
    "detalles": [
      {
        "idCompraVehiculoDetalle": 1,
        "idCompraVehiculo": 1,
        "idCompra": 5,
        "compraSerie": "C-2024-005",
        "compraFRegistro": "2024-01-10T08:00:00",
        "compraMontoTotal": 85000.00,
        "compraNegociacionId": "3"
      }
    ]
  }
}
```

### 3. Crear asignaci�n de compras a veh�culo
```http
POST /api/compras/asignacion-vehiculos
Content-Type: application/json

{
  "idVehiculo": 1,
  "guiaRemision": "GR-2024-001",
  "idCompras": [5, 6, 7]
}
```

**Response:**
```json
{
  "success": true,
  "message": "Asignaci�n de compras a veh�culo creada exitosamente",
  "data": {
    "idCompraVehiculo": 1,
    "idVehiculo": 1,
    "guiaRemision": "GR-2024-001",
    "fRegistro": "2024-01-15T10:30:00",
    "vehiculoPlaca": "ABC-123",
    "vehiculoMarca": "Volvo",
    "vehiculoModelo": "FH 500",
    "detalles": [
      {
        "idCompraVehiculoDetalle": 1,
        "idCompraVehiculo": 1,
        "idCompra": 5
      },
      {
        "idCompraVehiculoDetalle": 2,
        "idCompraVehiculo": 1,
        "idCompra": 6
      },
      {
        "idCompraVehiculoDetalle": 3,
        "idCompraVehiculo": 1,
        "idCompra": 7
      }
    ]
  }
}
```

## Validaciones Implementadas:

### CreateCompraVehiculo:
- ? Veh�culo debe existir y ser v�lido
- ? Gu�a de remisi�n es obligatoria (m�x. 50 caracteres)
- ? Gu�a de remisi�n no puede estar duplicada
- ? Debe asignar al menos una compra
- ? Todas las compras deben existir
- ? Todas las compras deben estar en estado ACTIVO
- ? Las compras no deben estar asignadas previamente a otro veh�culo
- ? Todos los IDs de compras deben ser v�lidos (mayor a 0)

## Entidades Relacionadas:
- **CompraVehiculo** ? Entidad principal (cabecera)
- **CompraVehiculoDetalle** ? Detalle (relaci�n compra-veh�culo)
- **Vehiculo** ? Veh�culo asignado
- **Compra** ? Compras asignadas al veh�culo

## Estructura de Base de Datos:

### CompraVehiculo (Cabecera)
```sql
CREATE TABLE CompraVehiculo (
    IdCompraVehiculo INT PRIMARY KEY IDENTITY,
    IdVehiculo INT NOT NULL,
    GuiaRemision NVARCHAR(50) NOT NULL,
    FRegistro DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdVehiculo) REFERENCES Vehiculo(IdVehiculo)
);
```

### CompraVehiculoDetalle (Detalle)
```sql
CREATE TABLE CompraVehiculoDetalle (
    IdCompraVehiculoDetalle INT PRIMARY KEY IDENTITY,
    IdCompraVehiculo INT NOT NULL,
    IdCompra INT NOT NULL,
    FOREIGN KEY (IdCompraVehiculo) REFERENCES CompraVehiculo(IdCompraVehiculo),
    FOREIGN KEY (IdCompra) REFERENCES Compra(IdCompra)
);
```

## Relaciones:

```
VEHICULO (1) ????? (N) COMPRA_VEHICULO
                         ?
                    (1) Cabecera
                         ?
                    (N) COMPRA_VEHICULO_DETALLE
                         ?
                    (1) Detalle
                         ?
                    (1) COMPRA
```

## Arquitectura Aplicada:
- ? Clean Architecture
- ? CQRS Pattern (Commands y Queries separadas)
- ? MediatR para desacoplar handlers
- ? Repository Pattern + Unit of Work
- ? AutoMapper para mapeo de DTOs
- ? FluentValidation para validaciones robustas
- ? Manejo de excepciones personalizado

## Flujo de Asignaci�n de Compras a Veh�culo:

```
1. NEGOCIACI�N APROBADA POR CONTADORA
   ?
2. SE CREA COMPRA AUTOM�TICAMENTE (Estado: ACTIVO)
   ?
3. ASIGNAR COMPRA(S) A VEH�CULO
   POST /api/compras/asignacion-vehiculos
   {
     "idVehiculo": 1,
     "guiaRemision": "GR-2024-001",
     "idCompras": [5, 6, 7]
   }
   ?
4. SE CREA CompraVehiculo (Cabecera)
   - IdVehiculo: 1
   - GuiaRemision: "GR-2024-001"
   - FRegistro: Autom�tico
   ?
5. SE CREAN CompraVehiculoDetalle (Detalles)
   - Detalle 1: IdCompra = 5
   - Detalle 2: IdCompra = 6
   - Detalle 3: IdCompra = 7
   ?
6. COMPRAS ASIGNADAS AL VEH�CULO
   ? Listas para transporte con gu�a de remisi�n
```

## Casos de Uso:

### Caso 1: Asignar una sola compra a un veh�culo
```http
POST /api/compras/asignacion-vehiculos
{
  "idVehiculo": 1,
  "guiaRemision": "GR-2024-001",
  "idCompras": [5]
}
```

### Caso 2: Asignar m�ltiples compras a un veh�culo
```http
POST /api/compras/asignacion-vehiculos
{
  "idVehiculo": 2,
  "guiaRemision": "GR-2024-002",
  "idCompras": [5, 6, 7, 8]
}
```

### Caso 3: Consultar todas las asignaciones de un veh�culo
```http
GET /api/compras/asignacion-vehiculos?idVehiculo=1
```

### Caso 4: Buscar por gu�a de remisi�n
```http
GET /api/compras/asignacion-vehiculos?guiaRemision=GR-2024
```

## Errores Comunes:

### Error: Compra ya asignada a otro veh�culo
```json
{
  "success": false,
  "message": "Validaci�n fallida",
  "errors": {
    "Compra": ["La compra con ID 5 ya est� asignada a un veh�culo"]
  }
}
```

### Error: Gu�a de remisi�n duplicada
```json
{
  "success": false,
  "message": "Validaci�n fallida",
  "errors": {
    "GuiaRemision": ["La gu�a de remisi�n 'GR-2024-001' ya est� registrada"]
  }
}
```

### Error: Compra no est� en estado ACTIVO
```json
{
  "success": false,
  "message": "Validaci�n fallida",
  "errors": {
    "Compra": ["La compra con ID 5 no est� en estado ACTIVO"]
  }
}
```

## Prioridad: ALTA
Las asignaciones de compras a veh�culos son esenciales para el control de transporte y trazabilidad.

## Estado: ? IMPLEMENTADO Y COMPILADO

**Fecha de implementaci�n:** 2024-01-15  
**Framework:** .NET 8  
**Patr�n:** CQRS + Repository Pattern  
**Validaciones:** FluentValidation  
**Mapeos:** AutoMapper
