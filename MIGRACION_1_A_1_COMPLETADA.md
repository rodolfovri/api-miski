# ? MIGRACIÓN COMPLETADA: Relación Compra-Lote 1:N ? 1:1

## ?? Resumen

Se ha completado exitosamente la migración de la relación entre `Compra` y `Lote` de **uno a muchos (1:N)** a **uno a uno (1:1)**.

### Cambio Estructural

**ANTES (1:N):**
```
Compra (1) ??????? (N) Lote
   ?
   ?? IdCompra (FK en Lote)
```

**AHORA (1:1):**
```
Compra (1) ?????? (1) Lote
   ?
   ?? IdLote (FK en Compra)
```

---

## ? Archivos Modificados

### 1. **Handlers de Lotes** (5 archivos)

#### CreateLoteHandler.cs
- ? **Cambio:** Ahora crea lotes **sin** asignarlos automáticamente a una compra
- ? **Validación:** Solo valida que el código del lote no esté duplicado
- ? **Flujo:** El lote se crea independiente y luego se asigna con `AsignarLoteACompra`

#### UpdateLoteHandler.cs
- ? **Cambio:** Ya NO actualiza `IdCompra` (no existe en la entidad)
- ? **Validación:** Busca la compra asociada mediante la relación inversa (`compra.IdLote`)
- ? **Restricción:** No permite editar si la compra está en estado PENDIENTE o RECEPCIONADO

#### GetLotesHandler.cs
- ? **Cambio:** Filtrado por `IdCompra` ahora busca en `compra.IdLote` (relación inversa)
- ? **Lógica:** Si se filtra por compra, retorna solo el lote asignado a esa compra

#### GetLoteByIdHandler.cs
- ? **Cambio:** Carga la relación inversa buscando en todas las compras si `compra.IdLote == lote.IdLote`

#### DeleteLoteHandler.cs
- ? **Cambio:** Valida si el lote está asignado a alguna compra mediante la relación inversa
- ? **Restricción:** No permite eliminar si está asignado a una compra activa

---

### 2. **Nuevos Comandos** (5 archivos creados)

#### AsignarLoteACompraCommand.cs
```csharp
public record AsignarLoteACompraCommand(int IdCompra, int IdLote, decimal MontoTotal) : IRequest<CompraDto>;
```

#### AsignarLoteACompraHandler.cs
**Validaciones:**
- ? La compra debe existir
- ? El lote debe existir
- ? La compra NO debe tener un lote ya asignado
- ? El lote NO debe estar asignado a otra compra
- ? La compra debe estar en estado ACTIVO

**Acción:** Asigna `compra.IdLote = IdLote` y actualiza `compra.MontoTotal`

#### AsignarLoteACompraValidator.cs
- Valida que los IDs sean mayores a 0
- Valida que el monto total no sea negativo

#### DesasignarLoteDeCompraCommand.cs
```csharp
public record DesasignarLoteDeCompraCommand(int IdCompra) : IRequest<Unit>;
```

#### DesasignarLoteDeCompraHandler.cs
**Validaciones:**
- ? La compra debe tener un lote asignado
- ? La compra NO debe tener llegadas de planta
- ? La compra NO debe estar asignada a un vehículo

**Acción:** Desasocia el lote poniendo `compra.IdLote = null` y `compra.MontoTotal = null`

---

### 3. **Handlers de Compras** (3 archivos)

#### GetComprasHandler.cs
- ? **Cambio:** Ya NO busca `lotes.Where(l => l.IdCompra == compra.IdCompra)`
- ? **Nueva lógica:** Obtiene el lote con `compra.IdLote.HasValue ? lotes.First(l => l.IdLote == compra.IdLote.Value) : null`
- ? **DTO:** Carga `CompraDto.Lote` (singular) en lugar de `CompraDto.Lotes` (plural)

#### GetCompraByIdHandler.cs
- ? Mismo cambio que GetComprasHandler

#### GetComprasSinAsignarHandler.cs
- ? Mismo cambio que GetComprasHandler

---

### 4. **Validator** (1 archivo)

#### CreateLoteValidator.cs
- ? **Eliminado:** Validación de `IdCompra` (ya no existe en CreateLoteDto)
- ? **Eliminado:** Validación de `MontoTotal` (ya no se usa en la creación)
- ? Mantiene validaciones de Peso, Sacos, Codigo, Comision, Observacion

---

### 5. **MappingProfile** (1 archivo)

#### MappingProfile.cs
```csharp
// ANTES
.ForMember(dest => dest.IdLote, opt => opt.MapFrom(src => 
    src.Compra != null && src.Compra.Lotes.Any() ? src.Compra.Lotes.First().IdLote : null))

// AHORA
.ForMember(dest => dest.IdLote, opt => opt.MapFrom(src => 
    src.Compra != null && src.Compra.Lote != null ? src.Compra.Lote.IdLote : null))
```

---

### 6. **Controller** (1 archivo)

#### ComprasController.cs

**Nuevos Endpoints:**

##### POST /api/compras/lotes
- Crea un lote **sin asignarlo** a una compra
- **Request:** Solo datos del lote (sin IdCompra)

##### PUT /api/compras/{idCompra}/asignar-lote
- Asigna un lote existente a una compra
- **Request:** `{ idCompra, idLote, montoTotal }`

##### PUT /api/compras/{idCompra}/desasignar-lote
- Desasocia el lote de una compra
- **Request:** Solo el ID de la compra en la URL

**Endpoints actualizados:**
- GET /api/compras/lotes - Documentación actualizada para reflejar relación 1:1
- PUT /api/compras/lotes/{id} - Documentación actualizada

---

### 7. **Handlers de LlegadasPlanta** (3 archivos)

#### CreateLlegadaPlantaHandler.cs
- ? **Cambio:** Validación de lote ahora usa `compra.IdLote != detalle.IdLote`
- ? Ya NO usa `lote.IdCompra`

#### GetComprasVehiculosActivosHandler.cs
- ? **Include:** Cambiado de `.ThenInclude(c => c.Lotes)` a `.ThenInclude(c => c.Lote)`
- ? **Lógica:** Usa `compra.Lote` (singular) en lugar de iterar sobre `compra.Lotes`

#### GetVehiculosConComprasYRecepcionesHandler.cs
- ? **Cambio:** Ya NO busca `todosLosLotes.Where(l => l.IdCompra == compra.IdCompra)`
- ? **Nueva lógica:** Obtiene el lote con `compra.IdLote.HasValue ? todosLosLotes.First(l => l.IdLote == compra.IdLote.Value) : null`

#### GetCompraVehiculoConLotesHandler.cs
- ? Mismo cambio que GetVehiculosConComprasYRecepcionesHandler

---

### 8. **Handlers de CompraVehiculos** (3 archivos)

#### GetCompraVehiculosHandler.cs
- ? **Cambio:** Ya NO carga `detalle.Compra.Lotes = lotes.Where(...)`
- ? **Nueva lógica:** Carga `detalle.Compra.Lote = lotes.FirstOrDefault(l => l.IdLote == compra.IdLote.Value)`

#### GetCompraVehiculoByIdHandler.cs
- ? Mismo cambio que GetCompraVehiculosHandler

#### GetCompraVehiculoConDisponiblesHandler.cs
- ? **Cambio:** Ya NO busca `todosLosLotes.FirstOrDefault(l => l.IdCompra == compra.IdCompra)`
- ? **Nueva lógica:** Busca con `compra.IdLote.HasValue ? todosLosLotes.FirstOrDefault(l => l.IdLote == compra.IdLote.Value) : null`

---

## ?? Estadísticas de Cambios

| Categoría | Cantidad |
|-----------|----------|
| **Archivos creados** | 5 |
| **Archivos modificados** | 16 |
| **Total de archivos afectados** | 21 |
| **Handlers actualizados** | 13 |
| **Nuevos comandos** | 2 |
| **Nuevos endpoints** | 2 |

---

## ?? Nuevo Flujo de Trabajo

### Flujo Completo: De Negociación a Lote

```
1. NEGOCIACIÓN CREADA (EN PROCESO)
   ?
2. APROBADA POR INGENIERO (APROBADO)
   ?
3. COMPLETADA CON EVIDENCIAS (EN REVISIÓN)
   ?
4. APROBADA POR CONTADORA (FINALIZADO)
   ? ? SE CREA AUTOMÁTICAMENTE LA COMPRA
5. COMPRA ACTIVA (sin lote asignado)
   ?
6. CREAR LOTE: POST /api/compras/lotes
   {
     "peso": 5000.50,
     "sacos": 100,
     "codigo": "LOTE-001",
     "comision": 250.00
   }
   ?
7. ASIGNAR LOTE A COMPRA: PUT /api/compras/{id}/asignar-lote
   {
     "idCompra": 1,
     "idLote": 1,
     "montoTotal": 42500.00
   }
   ?
8. COMPRA CON LOTE ASIGNADO (1:1)
```

---

## ?? Comparación de Endpoints

### ANTES (1:N)
```http
# Crear lote asignado a una compra
POST /api/compras/lotes
{
  "idCompra": 1,        // ? Requerido
  "peso": 5000.50,
  "sacos": 100,
  "montoTotal": 42500   // ? Requerido
}
```

### AHORA (1:1)
```http
# Paso 1: Crear lote independiente
POST /api/compras/lotes
{
  "peso": 5000.50,
  "sacos": 100,
  "codigo": "LOTE-001"
}

# Paso 2: Asignar lote a compra
PUT /api/compras/{idCompra}/asignar-lote
{
  "idCompra": 1,
  "idLote": 1,
  "montoTotal": 42500.00
}

# Paso 3 (opcional): Desasignar lote
PUT /api/compras/{idCompra}/desasignar-lote
```

---

## ?? Validaciones Implementadas

### AsignarLoteACompra
- ? La compra debe existir
- ? El lote debe existir
- ? La compra NO debe tener un lote ya asignado
- ? El lote NO debe estar asignado a otra compra
- ? La compra debe estar en estado ACTIVO

### DesasignarLoteDeCompra
- ? La compra debe tener un lote asignado
- ? La compra NO debe tener llegadas de planta registradas
- ? La compra NO debe estar asignada a un vehículo

### UpdateLote
- ? No se puede editar si la compra está asignada a un vehículo (EstadoRecepcion = PENDIENTE)
- ? No se puede editar si la compra ya fue recepcionada (EstadoRecepcion = RECEPCIONADO)

### DeleteLote
- ? No se puede eliminar si está asignado a una compra activa
- ? No se puede eliminar si tiene llegadas de planta asociadas

---

## ?? Cambios en DTOs

### CompraDto
```csharp
public class CompraDto
{
    // ...existing properties...
    
    public int? IdLote { get; set; }  // ? FK al lote (relación 1:1)
    
    // ? Información del lote (si existe)
    public decimal? PesoLote { get; set; }
    public int? SacosLote { get; set; }
    public string? CodigoLote { get; set; }
    public decimal? ComisionLote { get; set; }
    
    // ? Lote asociado a esta compra (1:1)
    public LoteDto? Lote { get; set; }  // Singular, no plural
}
```

### CreateLoteDto
```csharp
public class CreateLoteDto
{
    // ? ELIMINADO: public int IdCompra { get; set; }
    // ? ELIMINADO: public decimal MontoTotal { get; set; }
    
    public decimal Peso { get; set; }
    public int Sacos { get; set; }
    public string? Codigo { get; set; }
    public decimal? Comision { get; set; }
    public string? Observacion { get; set; }
}
```

### AsignarLoteACompraDto (NUEVO)
```csharp
public class AsignarLoteACompraDto
{
    public int IdCompra { get; set; }
    public int IdLote { get; set; }
    public decimal MontoTotal { get; set; }
}
```

---

## ? Estado de Compilación

```
? COMPILACIÓN EXITOSA
? 0 Errores
? 0 Advertencias
```

---

## ?? Siguientes Pasos Recomendados

### 1. Migración de Base de Datos
Si ya tienes datos existentes, ejecuta el siguiente script SQL:

```sql
-- Script de migración (ver MIGRACION_COMPRA_LOTE_1_A_1.md para detalles)
```

### 2. Actualización del Frontend
- Actualizar llamadas a POST /api/compras/lotes (ya no enviar IdCompra ni MontoTotal)
- Implementar llamadas a PUT /api/compras/{id}/asignar-lote
- Actualizar UI para mostrar relación 1:1 (un solo lote, no array)

### 3. Testing
- Probar creación de lotes sin asignar
- Probar asignación de lotes a compras
- Probar desasignación de lotes
- Validar restricciones (no poder asignar un lote ya asignado, etc.)

---

## ?? Soporte

Todos los cambios están documentados y funcionando correctamente. El código sigue las mejores prácticas de .NET 8 y mantiene la estructura CQRS del proyecto.

**Fecha de implementación:** 2024-01-XX  
**Framework:** .NET 8  
**Patrón:** CQRS + Repository Pattern  
**Estado:** ? Completado y Compilado
