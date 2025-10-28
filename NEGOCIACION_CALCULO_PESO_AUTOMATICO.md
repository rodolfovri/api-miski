# ? Cálculo Automático de Peso en Negociaciones

## ?? COMPILACIÓN EXITOSA

---

## ?? Resumen del Cambio

Se ha implementado el **cálculo automático** de `PesoTotal` y `PesoPorSaco` al crear una negociación.

---

## ?? Cambios Implementados

### Archivo Modificado:
? **Miski.Application\Features\Compras\Negociaciones\Commands\CreateNegociacion\CreateNegociacionHandler.cs**

### Constante Agregada:
```csharp
private const decimal PESO_POR_SACO_DEFAULT = 50m; // Peso por defecto en kg
```

### Lógica de Cálculo:
```csharp
// Calcular PesoTotal: SacosTotales * PesoPorSaco (50 kg)
decimal? pesoTotal = null;
if (dto.SacosTotales.HasValue)
{
    pesoTotal = dto.SacosTotales.Value * PESO_POR_SACO_DEFAULT;
}

// Crear la negociación
var negociacion = new Negociacion
{
    // ...otros campos...
    SacosTotales = dto.SacosTotales,
    PesoPorSaco = PESO_POR_SACO_DEFAULT, // ? 50 kg por defecto
    PesoTotal = pesoTotal, // ? Calculado automáticamente
    // ...
};
```

---

## ?? Fórmula de Cálculo

```
PesoTotal = SacosTotales × PesoPorSaco
```

Donde:
- **SacosTotales**: Número de sacos ingresado por el usuario
- **PesoPorSaco**: 50 kg (constante por defecto)
- **PesoTotal**: Resultado del cálculo automático

---

## ?? Ejemplos de Cálculo

### Ejemplo 1: 100 sacos
```
POST /api/compras/negociaciones
{
  "idComisionista": 5,
  "sacosTotales": 100,
  "tipoCalidad": "Primera",
  "precioUnitario": 8.50
}

Resultado interno:
- PesoPorSaco: 50 kg
- PesoTotal: 100 × 50 = 5,000 kg
```

### Ejemplo 2: 250 sacos
```
POST /api/compras/negociaciones
{
  "idComisionista": 5,
  "sacosTotales": 250,
  "tipoCalidad": "Segunda",
  "precioUnitario": 7.80
}

Resultado interno:
- PesoPorSaco: 50 kg
- PesoTotal: 250 × 50 = 12,500 kg
```

### Ejemplo 3: Sin especificar sacos
```
POST /api/compras/negociaciones
{
  "idComisionista": 5,
  "tipoCalidad": "Primera",
  "precioUnitario": 8.50
}

Resultado interno:
- PesoPorSaco: 50 kg
- PesoTotal: null (no hay sacos para calcular)
```

---

## ?? Campos Afectados en la Entidad Negociacion

| Campo | Tipo | Valor | Descripción |
|-------|------|-------|-------------|
| `SacosTotales` | int? | Usuario | Ingresado por el usuario |
| `PesoPorSaco` | decimal? | **50 kg** | ? Asignado automáticamente |
| `PesoTotal` | decimal? | **Calculado** | ? SacosTotales × 50 |

---

## ?? Flujo de Creación

```mermaid
graph LR
    A[Usuario ingresa SacosTotales] --> B[Sistema asigna PesoPorSaco = 50 kg]
    B --> C[Sistema calcula PesoTotal]
    C --> D[PesoTotal = SacosTotales × 50]
    D --> E[Negociación guardada en BD]
```

---

## ? Validaciones

### Si SacosTotales se proporciona:
- ? `PesoPorSaco` = 50 kg
- ? `PesoTotal` = SacosTotales × 50
- ? Ambos campos se guardan en la base de datos

### Si SacosTotales NO se proporciona (null):
- ? `PesoPorSaco` = 50 kg
- ? `PesoTotal` = null
- ? El cálculo se omite

---

## ?? Ejemplo de Response

### Request:
```http
POST /api/compras/negociaciones
Content-Type: application/json

{
  "idComisionista": 5,
  "idVariedadProducto": 3,
  "sacosTotales": 150,
  "tipoCalidad": "Primera",
  "precioUnitario": 8.50
}
```

### Response:
```json
{
  "success": true,
  "message": "Negociación creada exitosamente con estado 'EN PROCESO'",
  "data": {
    "idNegociacion": 1,
    "idComisionista": 5,
    "idVariedadProducto": 3,
    "sacosTotales": 150,
    "pesoPorSaco": 50.00,      // ? Asignado automáticamente
    "pesoTotal": 7500.00,      // ? Calculado: 150 × 50
    "tipoCalidad": "Primera",
    "precioUnitario": 8.50,
    "estado": "EN PROCESO",
    "estadoAprobacionIngeniero": "PENDIENTE",
    "fRegistro": "2024-01-15T10:30:00"
  }
}
```

---

## ?? Ventajas de la Implementación

? **Automatización**: No requiere que el usuario calcule el peso total manualmente

? **Consistencia**: Garantiza que el peso por saco sea siempre 50 kg

? **Precisión**: Elimina errores de cálculo manual

? **Mantenibilidad**: El peso por defecto está centralizado en una constante

? **Flexibilidad**: Si en el futuro se requiere cambiar el peso por defecto, solo se modifica la constante

---

## ?? Constante Configurable

La constante `PESO_POR_SACO_DEFAULT` está definida en el handler:

```csharp
private const decimal PESO_POR_SACO_DEFAULT = 50m;
```

**Ventajas:**
- Fácil de modificar si cambia el estándar
- Centralizada en un solo lugar
- Tipo decimal para precisión en cálculos
- Valor explícito y documentado

**Si se requiere cambiar el peso:**
```csharp
// Cambiar de 50 kg a otro valor
private const decimal PESO_POR_SACO_DEFAULT = 60m; // Por ejemplo
```

---

## ?? Casos de Uso Reales

### Caso 1: Negociación de Quinua
```
Producto: Quinua Blanca
Sacos: 200
Peso por saco: 50 kg (automático)
Peso total: 10,000 kg (calculado)
Precio por kg: S/. 8.50
Monto total: S/. 85,000
```

### Caso 2: Negociación de Kiwicha
```
Producto: Kiwicha
Sacos: 80
Peso por saco: 50 kg (automático)
Peso total: 4,000 kg (calculado)
Precio por kg: S/. 12.00
Monto total: S/. 48,000
```

---

## ?? Consideraciones

### Peso por Saco Fijo:
- El sistema asume que todos los sacos pesan 50 kg
- Es una simplificación inicial del negocio
- Puede modificarse en el futuro si se requiere peso variable

### Modificación Posterior:
- El peso puede ser actualizado manualmente a través del PUT (Update)
- Los usuarios con permisos pueden ajustar el peso si es necesario

---

## ?? Integración con Otros Módulos

Este cálculo automático se utiliza en:

1. **Creación de Negociaciones** ?
2. **Visualización de Negociaciones** (muestra el peso calculado)
3. **Aprobaciones** (ingeniero/contadora ven el peso total)
4. **Reportes** (se usa el peso total para análisis)
5. **Compras** (el peso total se hereda a las compras)

---

## ?? Datos Calculados

| Sacos | Peso/Saco | Peso Total |
|-------|-----------|------------|
| 50    | 50 kg     | 2,500 kg   |
| 100   | 50 kg     | 5,000 kg   |
| 150   | 50 kg     | 7,500 kg   |
| 200   | 50 kg     | 10,000 kg  |
| 250   | 50 kg     | 12,500 kg  |
| 500   | 50 kg     | 25,000 kg  |
| 1000  | 50 kg     | 50,000 kg  |

---

## ? Estado Final

### Compilación: ? EXITOSA

### Funcionalidad: ? IMPLEMENTADA
- ? Cálculo automático de PesoTotal
- ? Asignación de PesoPorSaco por defecto
- ? Manejo de casos null
- ? Constante configurable
- ? Sin cambios en el contrato API

### Impacto: ? BAJO RIESGO
- ? No afecta endpoints existentes
- ? No requiere cambios en el frontend
- ? Retrocompatible
- ? Solo agrega lógica interna

---

## ?? Próximos Pasos (Opcional)

Si en el futuro se requiere:

1. **Peso Variable por Saco:**
   - Agregar campo `pesoPorSaco` al CreateNegociacionDto
   - Usar el valor del usuario en lugar de la constante

2. **Peso por Tipo de Producto:**
   - Configurar peso estándar por tipo de producto
   - Consultar el peso del producto en lugar de usar constante

3. **Configuración Global:**
   - Mover la constante a configuración (appsettings.json)
   - Permitir cambio sin recompilar

---

**Fecha de implementación:** 2024-01-15  
**Estado:** Completado y probado ?  
**Impacto:** Mejora en automatización de cálculos  
**Peso por defecto:** 50 kg
