# ?? MIGRACIÓN: Relación Compra-Lote de Uno a Muchos (1:N) a Uno a Uno (1:1)

## ?? Resumen del Cambio

Se está cambiando la relación entre `Compra` y `Lote` de **uno a muchos** (una compra puede tener múltiples lotes) a **uno a uno** (una compra tiene exactamente un lote).

### ? ANTES (1:N)
```
Compra (1) ??????? (N) Lote
   ?
   ?? IdCompra (FK en Lote)
```

### ? DESPUÉS (1:1)
```
Compra (1) ?????? (1) Lote
   ?
   ?? IdLote (FK en Compra)
```

---

## ? Cambios Ya Aplicados

### 1. Entidades
- ? `Lote.cs` - Cambiado `ICollection<Compra>` a `Compra?`
- ? `Compra.cs` - Ya tiene `int? IdLote` y `Lote? Lote`

### 2. DbContext
- ? Configuración actualizada a relación 1:1:
```csharp
entity.HasOne(d => d.Lote)
    .WithOne(p => p.Compra)
    .HasForeignKey<Compra>(d => d.IdLote)
```

### 3. DTOs
- ? `CompraDto` - Actualizado para reflejar 1:1
- ? `LoteDto` - Actualizado
- ? `CreateLoteDto` - Simplificado (sin IdCompra)
- ? Nuevo: `AsignarLoteACompraDto`

---

## ?? Cambios Pendientes (Críticos)

### ?? Handlers que Necesitan Actualización

Todos estos archivos tienen lógica basada en la relación 1:N y deben actualizarse:

#### 1. **CreateLoteHandler.cs** ?
**Ruta:** `Miski.Application/Features/Compras/Lotes/Commands/CreateLote/`

**Problema Actual:**
```csharp
var lote = new Lote
{
    IdCompra = dto.IdCompra,  // ? Ya no existe IdCompra en Lote
    ...
};
```

**Solución:**
```csharp
// 1. Crear el lote SIN asignarlo a una compra
var lote = new Lote
{
    Peso = dto.Peso,
    Sacos = dto.Sacos,
    Codigo = dto.Codigo,
    Comision = dto.Comision,
    Observacion = dto.Observacion
};

await _unitOfWork.Repository<Lote>().AddAsync(lote, cancellationToken);
await _unitOfWork.SaveChangesAsync(cancellationToken);

// 2. LUEGO se asigna a una compra en un endpoint separado
```

#### 2. **UpdateLoteHandler.cs** ?
**Ruta:** `Miski.Application/Features/Compras/Lotes/Commands/UpdateLote/`

**Problema:**
```csharp
lote.IdCompra = dto.IdCompra;  // ? Ya no existe
```

**Solución:**
```csharp
// Solo actualizar campos del lote, no la relación con Compra
lote.Peso = dto.Peso;
lote.Sacos = dto.Sacos;
lote.Codigo = dto.Codigo;
lote.Comision = dto.Comision;
lote.Observacion = dto.Observacion;
```

#### 3. **Get Lotes**Handler.cs** ?
**Ruta:** `Miski.Application/Features/Compras/Lotes/Queries/GetLotes/`

**Problema:**
```csharp
if (request.IdCompra.HasValue)
{
    lotes = lotes.Where(l => l.IdCompra == request.IdCompra.Value);  // ?
}
```

**Solución:**
```csharp
// Obtener compra con su lote
if (request.IdCompra.HasValue)
{
    var compra = await _unitOfWork.Repository<Compra>()
        .GetByIdAsync(request.IdCompra.Value, cancellationToken);
    
    if (compra != null && compra.IdLote.HasValue)
    {
        var lote = await _unitOfWork.Repository<Lote>()
            .GetByIdAsync(compra.IdLote.Value, cancellationToken);
        
        return lote != null ? new List<LoteDto> { _mapper.Map<LoteDto>(lote) } : new List<LoteDto>();
    }
}
```

#### 4. **CompraVehiculoDetalle** - Handlers ?
**Problema:** Buscan lotes por `IdCompra`:
```csharp
var lote = todosLosLotes.FirstOrDefault(l => l.IdCompra == compra.IdCompra);  // ?
```

**Solución:**
```csharp
// Obtener el lote desde la compra
Lote? lote = null;
if (compra.IdLote.HasValue)
{
    lote = await _unitOfWork.Repository<Lote>()
        .GetByIdAsync(compra.IdLote.Value, cancellationToken);
}
```

---

## ?? Nuevo Flujo de Trabajo (1:1)

### Antes (1:N)
```
1. Crear Compra (sin lotes)
2. Crear Lote 1 para Compra
3. Crear Lote 2 para Compra
4. Crear Lote 3 para Compra
```

### Ahora (1:1)
```
1. Crear Compra (sin lote asignado)
2. Crear Lote (independiente)
3. Asignar Lote a Compra (relación 1:1)
```

---

## ?? Nuevos Endpoints Sugeridos

### 1. POST /api/compras/lotes
**Sin IdCompra, solo crea el lote**
```json
{
  "peso": 5000.50,
  "sacos": 100,
  "codigo": "LOTE-001",
  "comision": 250.00,
  "observacion": "Lote de quinua blanca"
}
```

### 2. PUT /api/compras/{idCompra}/asignar-lote
**Asigna un lote existente a una compra**
```json
{
  "idCompra": 1,
  "idLote": 5,
  "montoTotal": 42500.00
}
```

**Validaciones:**
- ? La compra no debe tener un lote asignado
- ? El lote no debe estar asignado a otra compra
- ? Estado de la compra debe ser ACTIVO

### 3. PUT /api/compras/{idCompra}/desasignar-lote
**Desasocia el lote de una compra**
```json
{
  "idCompra": 1
}
```

**Validaciones:**
- ? La compra no debe tener LlegadasPlanta
- ? La compra no debe estar en un CompraVehiculo

---

## ?? Archivos que Requieren Cambios

### Alto Impacto (Crítico)
1. ? `CreateLoteHandler.cs`
2. ? `CreateLoteValidator.cs`
3. ? `UpdateLoteHandler.cs`
4. ? `UpdateLoteValidator.cs`
5. ? `GetLotesHandler.cs`
6. ? `GetLoteByIdHandler.cs`
7. ? `DeleteLoteHandler.cs`

### Medio Impacto
8. ? `GetCompraVehiculoConDisponiblesHandler.cs`
9. ? `GetCompraVehiculosHandler.cs`
10. ? `GetCompraByIdHandler.cs`
11. ? `GetComprasHandler.cs`

### Nuevos Handlers Necesarios
12. ? **Crear:** `AsignarLoteACompraHandler.cs`
13. ? **Crear:** `DesasignarLoteDeCompraHandler.cs`

### Controller
14. ? `ComprasController.cs` - Actualizar todos los endpoints de lotes

### Mappings
15. ? `MappingProfile.cs` - Actualizar mapeos

---

## ??? Migración de Base de Datos

Si ya tienes datos existentes, necesitarás una migración:

```sql
-- 1. Crear columna temporal en Lote
ALTER TABLE Lote ADD IdCompra_Old INT NULL;

-- 2. Copiar datos existentes
UPDATE L
SET L.IdCompra_Old = L.IdCompra
FROM Lote L;

-- 3. Eliminar relación anterior (FK de Lote a Compra)
ALTER TABLE Lote DROP CONSTRAINT IF EXISTS FK_Lote_Compra;
ALTER TABLE Lote DROP COLUMN IF EXISTS IdCompra;

-- 4. Agregar nueva relación (FK de Compra a Lote)
ALTER TABLE Compra ADD IdLote INT NULL;

-- 5. Migrar datos: Asignar el primer lote de cada compra
UPDATE C
SET C.IdLote = (
    SELECT TOP 1 IdLote 
    FROM Lote L 
    WHERE L.IdCompra_Old = C.IdCompra
    ORDER BY IdLote ASC
)
FROM Compra C;

-- 6. Los demás lotes quedan sin asignar (se pueden eliminar o reasignar manualmente)

-- 7. Crear FK
ALTER TABLE Compra ADD CONSTRAINT FK_Compra_Lote 
    FOREIGN KEY (IdLote) REFERENCES Lote(IdLote);

-- 8. Limpiar columna temporal
ALTER TABLE Lote DROP COLUMN IdCompra_Old;
```

---

## ?? Consideraciones para el Panel Web

### Cambios en el Frontend

#### Antes (1:N)
```javascript
// Obtener compra con sus lotes
GET /api/compras/1
{
  "idCompra": 1,
  "lotes": [  // ? Ya no es un array
    { "idLote": 1, "peso": 5000 },
    { "idLote": 2, "peso": 3000 }
  ]
}
```

#### Ahora (1:1)
```javascript
// Obtener compra con su lote
GET /api/compras/1
{
  "idCompra": 1,
  "idLote": 1,  // ? Solo un ID
  "lote": {  // ? Solo un objeto
    "idLote": 1,
    "peso": 5000,
    "sacos": 100
  }
}
```

### UI Sugerida

#### Página de Compras
```
Compra #1 - Estado: ACTIVO
?? Serie: CMP-2024-001
?? Negociación: #25
?? Lote Asignado: [Seleccionar Lote ?]
   
   [Botón: Asignar Lote]
```

#### Página de Lotes
```
Lotes Disponibles
?? LOTE-001 (5000 kg, 100 sacos) - Sin Asignar
?? LOTE-002 (3000 kg, 60 sacos) - Asignado a Compra #5
?? LOTE-003 (4500 kg, 90 sacos) - Sin Asignar

[Botón: Crear Nuevo Lote]
```

---

## ? Plan de Acción Recomendado

### Fase 1: Preparación
1. ? Respaldar base de datos
2. ? Crear branch de desarrollo
3. ? Actualizar entidades (ya hecho)
4. ? Actualizar DbContext (ya hecho)

### Fase 2: Backend
5. ? Actualizar todos los Handlers de Lote
6. ? Crear nuevos comandos (AsignarLote, DesasignarLote)
7. ? Actualizar Validators
8. ? Actualizar MappingProfile
9. ? Actualizar Controller

### Fase 3: Testing
10. ? Pruebas unitarias
11. ? Pruebas de integración
12. ? Probar endpoints con Postman/Swagger

### Fase 4: Migración de Datos
13. ? Ejecutar script de migración SQL
14. ? Verificar integridad de datos
15. ? Validar relaciones

### Fase 5: Frontend
16. ? Actualizar servicios/APIs en el panel web
17. ? Actualizar componentes que muestran lotes
18. ? Actualizar formularios de creación/edición

---

## ?? Riesgos y Mitigaciones

| Riesgo | Impacto | Mitigación |
|--------|---------|------------|
| Pérdida de lotes existentes | Alto | Migración SQL cuidadosa |
| Panel web rompe | Alto | Mantener compatibilidad temporal |
| Queries lentas | Medio | Indexar `Compra.IdLote` |
| Datos huérfanos | Medio | Script de limpieza post-migración |

---

## ?? ¿Necesitas Ayuda?

Si quieres que implemente los cambios en los handlers, confirma y procederé con:
1. ? Actualizar CreateLoteHandler
2. ? Actualizar UpdateLoteHandler
3. ? Actualizar GetLotesHandler
4. ? Crear AsignarLoteACompraHandler
5. ? Crear DesasignarLoteDeCompraHandler
6. ? Actualizar ComprasController
7. ? Actualizar MappingProfile

**¿Deseas que proceda con la implementación completa?**

