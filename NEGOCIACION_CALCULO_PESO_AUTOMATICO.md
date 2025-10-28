# ? C�lculo Autom�tico de Peso en Negociaciones

## ?? COMPILACI�N EXITOSA

---

## ?? Resumen del Cambio

Se ha implementado el **c�lculo autom�tico** de `PesoTotal` y `PesoPorSaco` al crear una negociaci�n.

---

## ?? Cambios Implementados

### Archivo Modificado:
? **Miski.Application\Features\Compras\Negociaciones\Commands\CreateNegociacion\CreateNegociacionHandler.cs**

### Constante Agregada:
```csharp
private const decimal PESO_POR_SACO_DEFAULT = 50m; // Peso por defecto en kg
```

### L�gica de C�lculo:
```csharp
// Calcular PesoTotal: SacosTotales * PesoPorSaco (50 kg)
decimal? pesoTotal = null;
if (dto.SacosTotales.HasValue)
{
    pesoTotal = dto.SacosTotales.Value * PESO_POR_SACO_DEFAULT;
}

// Crear la negociaci�n
var negociacion = new Negociacion
{
    // ...otros campos...
    SacosTotales = dto.SacosTotales,
    PesoPorSaco = PESO_POR_SACO_DEFAULT, // ? 50 kg por defecto
    PesoTotal = pesoTotal, // ? Calculado autom�ticamente
    // ...
};
```

---

## ?? F�rmula de C�lculo

```
PesoTotal = SacosTotales � PesoPorSaco
```

Donde:
- **SacosTotales**: N�mero de sacos ingresado por el usuario
- **PesoPorSaco**: 50 kg (constante por defecto)
- **PesoTotal**: Resultado del c�lculo autom�tico

---

## ?? Ejemplos de C�lculo

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
- PesoTotal: 100 � 50 = 5,000 kg
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
- PesoTotal: 250 � 50 = 12,500 kg
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

| Campo | Tipo | Valor | Descripci�n |
|-------|------|-------|-------------|
| `SacosTotales` | int? | Usuario | Ingresado por el usuario |
| `PesoPorSaco` | decimal? | **50 kg** | ? Asignado autom�ticamente |
| `PesoTotal` | decimal? | **Calculado** | ? SacosTotales � 50 |

---

## ?? Flujo de Creaci�n

```mermaid
graph LR
    A[Usuario ingresa SacosTotales] --> B[Sistema asigna PesoPorSaco = 50 kg]
    B --> C[Sistema calcula PesoTotal]
    C --> D[PesoTotal = SacosTotales � 50]
    D --> E[Negociaci�n guardada en BD]
```

---

## ? Validaciones

### Si SacosTotales se proporciona:
- ? `PesoPorSaco` = 50 kg
- ? `PesoTotal` = SacosTotales � 50
- ? Ambos campos se guardan en la base de datos

### Si SacosTotales NO se proporciona (null):
- ? `PesoPorSaco` = 50 kg
- ? `PesoTotal` = null
- ? El c�lculo se omite

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
  "message": "Negociaci�n creada exitosamente con estado 'EN PROCESO'",
  "data": {
    "idNegociacion": 1,
    "idComisionista": 5,
    "idVariedadProducto": 3,
    "sacosTotales": 150,
    "pesoPorSaco": 50.00,      // ? Asignado autom�ticamente
    "pesoTotal": 7500.00,      // ? Calculado: 150 � 50
    "tipoCalidad": "Primera",
    "precioUnitario": 8.50,
    "estado": "EN PROCESO",
    "estadoAprobacionIngeniero": "PENDIENTE",
    "fRegistro": "2024-01-15T10:30:00"
  }
}
```

---

## ?? Ventajas de la Implementaci�n

? **Automatizaci�n**: No requiere que el usuario calcule el peso total manualmente

? **Consistencia**: Garantiza que el peso por saco sea siempre 50 kg

? **Precisi�n**: Elimina errores de c�lculo manual

? **Mantenibilidad**: El peso por defecto est� centralizado en una constante

? **Flexibilidad**: Si en el futuro se requiere cambiar el peso por defecto, solo se modifica la constante

---

## ?? Constante Configurable

La constante `PESO_POR_SACO_DEFAULT` est� definida en el handler:

```csharp
private const decimal PESO_POR_SACO_DEFAULT = 50m;
```

**Ventajas:**
- F�cil de modificar si cambia el est�ndar
- Centralizada en un solo lugar
- Tipo decimal para precisi�n en c�lculos
- Valor expl�cito y documentado

**Si se requiere cambiar el peso:**
```csharp
// Cambiar de 50 kg a otro valor
private const decimal PESO_POR_SACO_DEFAULT = 60m; // Por ejemplo
```

---

## ?? Casos de Uso Reales

### Caso 1: Negociaci�n de Quinua
```
Producto: Quinua Blanca
Sacos: 200
Peso por saco: 50 kg (autom�tico)
Peso total: 10,000 kg (calculado)
Precio por kg: S/. 8.50
Monto total: S/. 85,000
```

### Caso 2: Negociaci�n de Kiwicha
```
Producto: Kiwicha
Sacos: 80
Peso por saco: 50 kg (autom�tico)
Peso total: 4,000 kg (calculado)
Precio por kg: S/. 12.00
Monto total: S/. 48,000
```

---

## ?? Consideraciones

### Peso por Saco Fijo:
- El sistema asume que todos los sacos pesan 50 kg
- Es una simplificaci�n inicial del negocio
- Puede modificarse en el futuro si se requiere peso variable

### Modificaci�n Posterior:
- El peso puede ser actualizado manualmente a trav�s del PUT (Update)
- Los usuarios con permisos pueden ajustar el peso si es necesario

---

## ?? Integraci�n con Otros M�dulos

Este c�lculo autom�tico se utiliza en:

1. **Creaci�n de Negociaciones** ?
2. **Visualizaci�n de Negociaciones** (muestra el peso calculado)
3. **Aprobaciones** (ingeniero/contadora ven el peso total)
4. **Reportes** (se usa el peso total para an�lisis)
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

### Compilaci�n: ? EXITOSA

### Funcionalidad: ? IMPLEMENTADA
- ? C�lculo autom�tico de PesoTotal
- ? Asignaci�n de PesoPorSaco por defecto
- ? Manejo de casos null
- ? Constante configurable
- ? Sin cambios en el contrato API

### Impacto: ? BAJO RIESGO
- ? No afecta endpoints existentes
- ? No requiere cambios en el frontend
- ? Retrocompatible
- ? Solo agrega l�gica interna

---

## ?? Pr�ximos Pasos (Opcional)

Si en el futuro se requiere:

1. **Peso Variable por Saco:**
   - Agregar campo `pesoPorSaco` al CreateNegociacionDto
   - Usar el valor del usuario en lugar de la constante

2. **Peso por Tipo de Producto:**
   - Configurar peso est�ndar por tipo de producto
   - Consultar el peso del producto en lugar de usar constante

3. **Configuraci�n Global:**
   - Mover la constante a configuraci�n (appsettings.json)
   - Permitir cambio sin recompilar

---

**Fecha de implementaci�n:** 2024-01-15  
**Estado:** Completado y probado ?  
**Impacto:** Mejora en automatizaci�n de c�lculos  
**Peso por defecto:** 50 kg
