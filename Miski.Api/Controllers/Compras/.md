# Módulo de Llegadas a Planta

Este módulo gestiona el registro y seguimiento de llegadas de compras a la planta, con control detallado de lotes recibidos.

## Funcionalidades principales:
- Registrar llegadas de compras a planta
- Control de sacos y peso recibido por lote
- Cálculo automático de diferencias (asignado vs recibido)
- Consulta de lotes por vehículo
- Listado de todas las llegadas con filtros
- Trazabilidad completa de recepciones

## Controladores:
- `LlegadaPlantaController`: Gestión de llegadas a planta

## Estructura del módulo:

### Controllers (API Layer)
```
Miski.Api/Controllers/Compras/
??? LlegadaPlantaController.cs
```

### Features (Application Layer)
```
Miski.Application/Features/Compras/LlegadasPlanta/
??? Commands/
?   ??? CreateLlegadaPlanta/
?       ??? CreateLlegadaPlantaCommand.cs
?       ??? CreateLlegadaPlantaHandler.cs
?       ??? CreateLlegadaPlantaValidator.cs
??? Queries/
    ??? GetLlegadasPlanta/
    ?   ??? GetLlegadasPlantaQuery.cs
    ?   ??? GetLlegadasPlantaHandler.cs
    ??? GetLlegadaPlantaById/
    ?   ??? GetLlegadaPlantaByIdQuery.cs
    ?   ??? GetLlegadaPlantaByIdHandler.cs
    ??? GetCompraVehiculoConLotes/
        ??? GetCompraVehiculoConLotesQuery.cs
        ??? GetCompraVehiculoConLotesHandler.cs
```

### DTOs (Shared Layer)
```
Miski.Shared/DTOs/Compras/
??? LlegadaPlantaDto.cs
    ??? LlegadaPlantaDto
    ??? LlegadaPlantaDetalleDto
    ??? CreateLlegadaPlantaDto
    ??? CreateLlegadaPlantaDetalleDto
    ??? CompraVehiculoConLotesDto
    ??? CompraConLotesDto
    ??? LoteConRecepcionDto
```

## Endpoints Disponibles:

### 1. Obtener todas las llegadas a planta
```http
GET /api/compras/llegadas-planta
Query params:
  ?idCompra=1
  &estado=REGISTRADO
  &fechaInicio=2024-01-01
  &fechaFin=2024-12-31
```

**Response:**
```json
{
  "success": true,
  "message": "Llegadas a planta obtenidas exitosamente",
  "data": [
    {
      "idLlegadaPlanta": 1,
      "idCompra": 5,
      "idUsuario": 10,
      "fLlegada": "2024-01-20T14:30:00",
      "observaciones": "Llegada sin contratiempos",
      "estado": "REGISTRADO",
      "compraSerie": "C-2024-005",
      "usuarioNombre": "Juan Pérez García",
      "detalles": [
        {
          "idLlegadaDetalle": 1,
          "idLlegadaPlanta": 1,
          "idLote": 1,
          "sacosRecibidos": 28,
          "pesoRecibido": 1400.00,
          "observaciones": "2 sacos en mal estado",
          "loteCodigo": "L-2024-001",
          "sacosAsignados": 30,
          "pesoAsignado": 1500.00,
          "diferenciaSacos": 2,
          "diferenciaPeso": 100.00
        },
        {
          "idLlegadaDetalle": 2,
          "idLlegadaPlanta": 1,
          "idLote": 2,
          "sacosRecibidos": 40,
          "pesoRecibido": 2000.00,
          "observaciones": null,
          "loteCodigo": "L-2024-002",
          "sacosAsignados": 40,
          "pesoAsignado": 2000.00,
          "diferenciaSacos": 0,
          "diferenciaPeso": 0.00
        }
      ]
    },
    {
      "idLlegadaPlanta": 2,
      "idCompra": 6,
      "idUsuario": 10,
      "fLlegada": "2024-01-22T10:15:00",
      "observaciones": "Demora en descarga",
      "estado": "REGISTRADO",
      "compraSerie": "C-2024-006",
      "usuarioNombre": "Juan Pérez García",
      "detalles": [
        {
          "idLlegadaDetalle": 3,
          "idLlegadaPlanta": 2,
          "idLote": 3,
          "sacosRecibidos": 50,
          "pesoRecibido": 2500.00,
          "observaciones": null,
          "loteCodigo": "L-2024-003",
          "sacosAsignados": 50,
          "pesoAsignado": 2500.00,
          "diferenciaSacos": 0,
          "diferenciaPeso": 0.00
        }
      ]
    }
  ]
}
```

### 2. Obtener llegada a planta por ID
```http
GET /api/compras/llegadas-planta/{id}
```

**Response:**
```json
{
  "success": true,
  "message": "Llegada a planta obtenida exitosamente",
  "data": {
    "idLlegadaPlanta": 1,
    "idCompra": 5,
    "idUsuario": 10,
    "fLlegada": "2024-01-20T14:30:00",
    "observaciones": "Llegada sin contratiempos",
    "estado": "REGISTRADO",
    "compraSerie": "C-2024-005",
    "usuarioNombre": "Juan Pérez García",
    "detalles": [
      {
        "idLlegadaDetalle": 1,
        "idLlegadaPlanta": 1,
        "idLote": 1,
        "sacosRecibidos": 28,
        "pesoRecibido": 1400.00,
        "observaciones": "2 sacos en mal estado",
        "loteCodigo": "L-2024-001",
        "sacosAsignados": 30,
        "pesoAsignado": 1500.00,
        "diferenciaSacos": 2,
        "diferenciaPeso": 100.00
      }
    ]
  }
}
```

### 3. Obtener compras con lotes por vehículo
```http
GET /api/compras/llegadas-planta/por-vehiculo/{idCompraVehiculo}
```

**Response:**
```json
{
  "success": true,
  "message": "Compras con lotes obtenidas exitosamente",
  "data": {
    "idCompraVehiculo": 1,
    "idVehiculo": 2,
    "guiaRemision": "GR-2024-001",
    "fRegistro": "2024-01-15T10:00:00",
    "vehiculoPlaca": "ABC-123",
    "vehiculoMarca": "Volvo",
    "vehiculoModelo": "FH 500",
    "compras": [
      {
        "idCompra": 5,
        "serie": "C-2024-005",
        "fRegistro": "2024-01-10T08:00:00",
        "lotes": [
          {
            "idLote": 1,
            "codigo": "L-2024-001",
            "sacosAsignados": 30,
            "pesoAsignado": 1500.00,
            "idLlegadaDetalle": 1,
            "sacosRecibidos": 28,
            "pesoRecibido": 1400.00,
            "observaciones": "2 sacos en mal estado",
            "yaRecibido": true
          },
          {
            "idLote": 2,
            "codigo": "L-2024-002",
            "sacosAsignados": 40,
            "pesoAsignado": 2000.00,
            "idLlegadaDetalle": null,
            "sacosRecibidos": null,
            "pesoRecibido": null,
            "observaciones": null,
            "yaRecibido": false
          }
        ]
      }
    ]
  }
}
```

### 4. Registrar llegada a planta
```http
POST /api/compras/llegadas-planta
Content-Type: application/json

{
  "idCompra": 5,
  "idUsuario": 10,
  "fLlegada": "2024-01-20T14:30:00",
  "observaciones": "Llegada sin contratiempos",
  "detalles": [
    {
      "idLote": 1,
      "sacosRecibidos": 28,
      "pesoRecibido": 1400.00,
      "observaciones": "2 sacos en mal estado"
    },
    {
      "idLote": 2,
      "sacosRecibidos": 40,
      "pesoRecibido": 2000.00,
      "observaciones": null
    }
  ]
}
```

**Response:**
```json
{
  "success": true,
  "message": "Llegada a planta registrada exitosamente",
  "data": {
    "idLlegadaPlanta": 1,
    "idCompra": 5,
    "idUsuario": 10,
    "fLlegada": "2024-01-20T14:30:00",
    "observaciones": "Llegada sin contratiempos",
    "estado": "REGISTRADO",
    "compraSerie": "C-2024-005",
    "usuarioNombre": "Juan Pérez García",
    "detalles": [
      {
        "idLlegadaDetalle": 1,
        "idLlegadaPlanta": 1,
        "idLote": 1,
        "sacosRecibidos": 28,
        "pesoRecibido": 1400.00,
        "observaciones": "2 sacos en mal estado",
        "loteCodigo": "L-2024-001",
        "sacosAsignados": 30,
        "pesoAsignado": 1500.00,
        "diferenciaSacos": 2,
        "diferenciaPeso": 100.00
      },
      {
        "idLlegadaDetalle": 2,
        "idLlegadaPlanta": 1,
        "idLote": 2,
        "sacosRecibidos": 40,
        "pesoRecibido": 2000.00,
        "observaciones": null,
        "loteCodigo": "L-2024-002",
        "sacosAsignados": 40,
        "pesoAsignado": 2000.00,
        "diferenciaSacos": 0,
        "diferenciaPeso": 0.00
      }
    ]
  }
}
```

## Validaciones Implementadas:

### CreateLlegadaPlanta:
- ? ID de compra obligatorio
- ? ID de usuario obligatorio
- ? Fecha de llegada obligatoria
- ? Al menos un detalle de lote
- ? Todos los lotes deben existir
- ? Todos los lotes deben pertenecer a la compra especificada
- ? No se permiten lotes duplicados en una misma llegada
- ? Sacos recibidos >= 0
- ? Peso recibido >= 0

## Estructura de Base de Datos:

### LlegadaPlanta (Cabecera)
```sql
CREATE TABLE LlegadaPlanta (
    IdLlegadaPlanta INT PRIMARY KEY IDENTITY,
    IdCompra INT NOT NULL,
    IdUsuario INT NOT NULL,
    FLlegada DATETIME,
    Observaciones NVARCHAR(MAX),
    Estado NVARCHAR(50),
    FOREIGN KEY (IdCompra) REFERENCES Compra(IdCompra),
    FOREIGN KEY (IdUsuario) REFERENCES Persona(IdPersona)
);
```

### LlegadaPlantaDetalle (Detalle)
```sql
CREATE TABLE LlegadaPlantaDetalle (
    IdLlegadaDetalle INT PRIMARY KEY IDENTITY,
    IdLlegadaPlanta INT NOT NULL,
    IdLote INT NOT NULL,
    SacosRecibidos INT NOT NULL,
    PesoRecibido DECIMAL(18,2) NOT NULL,
    Observaciones NVARCHAR(MAX),
    FOREIGN KEY (IdLlegadaPlanta) REFERENCES LlegadaPlanta(IdLlegadaPlanta),
    FOREIGN KEY (IdLote) REFERENCES Lote(IdLote)
);
```

## Relaciones:

```
COMPRA (1) ??? (N) LLEGADA_PLANTA
                    ?
               (1) Cabecera
                    ?
               (N) LLEGADA_PLANTA_DETALLE
                    ?
               (1) Detalle
                    ?
               (1) LOTE
```

## Flujo de Proceso:

```
1. NEGOCIACIÓN APROBADA
   ?
2. SE CREA COMPRA (Estado: ACTIVO)
   ?
3. SE CREAN LOTES DE LA COMPRA
   ?
4. SE ASIGNA COMPRA A VEHÍCULO (CompraVehiculo)
   ?
5. VEHÍCULO LLEGA A PLANTA
   ?
6. CONSULTAR LOTES DEL VEHÍCULO
   GET /api/compras/llegadas-planta/por-vehiculo/{idCompraVehiculo}
   ?
7. REGISTRAR LLEGADA A PLANTA
   POST /api/compras/llegadas-planta
   {
     "idCompra": 5,
     "idUsuario": 10,
     "fLlegada": "2024-01-20T14:30:00",
     "detalles": [...]
   }
   ?
8. SE REGISTRA LlegadaPlanta (Cabecera)
   - IdCompra, IdUsuario, FLlegada
   - Estado: "REGISTRADO"
   ?
9. SE REGISTRAN LlegadaPlantaDetalle (Detalles)
   - Por cada lote: SacosRecibidos, PesoRecibido
   - Se calculan diferencias automáticamente
   ?
10. LLEGADA REGISTRADA
    ? Trazabilidad completa de recepción
```

## Casos de Uso:

### Caso 1: Registrar llegada completa sin faltantes
```http
POST /api/compras/llegadas-planta
{
  "idCompra": 5,
  "idUsuario": 10,
  "fLlegada": "2024-01-20T14:30:00",
  "observaciones": "Llegada completa",
  "detalles": [
    {
      "idLote": 1,
      "sacosRecibidos": 30,
      "pesoRecibido": 1500.00
    },
    {
      "idLote": 2,
      "sacosRecibidos": 40,
      "pesoRecibido": 2000.00
    }
  ]
}
```

### Caso 2: Registrar llegada con faltantes
```http
POST /api/compras/llegadas-planta
{
  "idCompra": 5,
  "idUsuario": 10,
  "fLlegada": "2024-01-20T14:30:00",
  "observaciones": "Faltan sacos",
  "detalles": [
    {
      "idLote": 1,
      "sacosRecibidos": 28,
      "pesoRecibido": 1400.00,
      "observaciones": "Faltan 2 sacos, reportado al transportista"
    }
  ]
}
```

### Caso 3: Consultar lotes de un vehículo antes de registrar
```http
GET /api/compras/llegadas-planta/por-vehiculo/1
```
Muestra todos los lotes asignados al vehículo e indica cuáles ya fueron recibidos.

### Caso 4: Consultar detalle de una llegada registrada
```http
GET /api/compras/llegadas-planta/1
```
Muestra la información completa con diferencias calculadas.

## Cálculos Automáticos:

### Diferencia de Sacos
```
DiferenciaSacos = SacosAsignados - SacosRecibidos
```
- Positivo: Faltaron sacos
- Cero: Cantidad exacta
- Negativo: Sacos de más

### Diferencia de Peso
```
DiferenciaPeso = PesoAsignado - PesoRecibido
```
- Positivo: Falta peso
- Cero: Peso exacto
- Negativo: Peso de más

## Errores Comunes:

### Error: Compra no encontrada
```json
{
  "success": false,
  "message": "Entidad relacionada no encontrada",
  "errors": {
    "": ["No se encontró Compra con ID 999"]
  }
}
```

### Error: Lote no pertenece a la compra
```json
{
  "success": false,
  "message": "Validación fallida",
  "errors": {
    "Lote": ["El lote con ID 5 no pertenece a la compra especificada"]
  }
}
```

### Error: Lotes duplicados
```json
{
  "success": false,
  "message": "Validación fallida",
  "errors": {
    "Detalles": ["No se pueden registrar lotes duplicados en una misma llegada"]
  }
}
```

### Error: Sin detalles
```json
{
  "success": false,
  "message": "Validación fallida",
  "errors": {
    "Detalles": ["Debe incluir al menos un detalle de lote recibido"]
  }
}
```

## Arquitectura Aplicada:
- ? Clean Architecture
- ? CQRS Pattern (Commands y Queries separadas)
- ? MediatR para desacoplar handlers
- ? Repository Pattern + Unit of Work
- ? FluentValidation para validaciones robustas
- ? Manejo de excepciones personalizado
- ? Cálculo automático de diferencias

## Estado: ? IMPLEMENTADO Y COMPILADO

**Fecha de implementación:** 2024  
**Framework:** .NET 8  
**Patrón:** CQRS + Repository Pattern  
**Validaciones:** FluentValidation  

## Prioridad: ALTA
El control de llegadas a planta es esencial para la trazabilidad y gestión de inventario.
