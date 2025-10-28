# ? Actualizaci�n: IdTipoDocumento e IdBanco en Completar Negociaci�n

## ?? COMPILACI�N EXITOSA

---

## ?? Resumen de Cambios

Se han agregado dos nuevos campos opcionales al endpoint de **Completar Negociaci�n**:
- `IdTipoDocumento` - Tipo de documento del proveedor
- `IdBanco` - Banco de la cuenta bancaria

---

## ?? Archivos Modificados

### 1. DTOs (1 archivo)
? **Miski.Shared\DTOs\Compras\NegociacionDto.cs**

#### CompletarNegociacionDto - ANTES:
```csharp
public class CompletarNegociacionDto
{
    public int IdNegociacion { get; set; }
    public string NroDocumentoProveedor { get; set; } = string.Empty;
    public string NroCuentaBancaria { get; set; } = string.Empty;
    
    // Fotos y video
    public IFormFile FotoDniFrontal { get; set; } = null!;
    public IFormFile FotoDniPosterior { get; set; } = null!;
    public IFormFile PrimeraEvindenciaFoto { get; set; } = null!;
    public IFormFile SegundaEvindenciaFoto { get; set; } = null!;
    public IFormFile TerceraEvindenciaFoto { get; set; } = null!;
    public IFormFile EvidenciaVideo { get; set; } = null!;
}
```

#### CompletarNegociacionDto - DESPU�S:
```csharp
public class CompletarNegociacionDto
{
    public int IdNegociacion { get; set; }
    public int? IdTipoDocumento { get; set; }  // ? AGREGADO
    public int? IdBanco { get; set; }  // ? AGREGADO
    public string NroDocumentoProveedor { get; set; } = string.Empty;
    public string NroCuentaBancaria { get; set; } = string.Empty;
    
    // Fotos y video
    public IFormFile FotoDniFrontal { get; set; } = null!;
    public IFormFile FotoDniPosterior { get; set; } = null!;
    public IFormFile PrimeraEvindenciaFoto { get; set; } = null!;
    public IFormFile SegundaEvindenciaFoto { get; set; } = null!;
    public IFormFile TerceraEvindenciaFoto { get; set; } = null!;
    public IFormFile EvidenciaVideo { get; set; } = null!;
}
```

#### NegociacionDto:
```csharp
public class NegociacionDto
{
    // ...campos existentes...
    public int? IdTipoDocumento { get; set; }  // ? AGREGADO
    public int? IdBanco { get; set; }  // ? AGREGADO
    // ...
    public string? TipoDocumentoNombre { get; set; }  // ? AGREGADO (info adicional)
    public string? BancoNombre { get; set; }  // ? AGREGADO (info adicional)
}
```

### 2. Handler (1 archivo)
? **Miski.Application\Features\Compras\Negociaciones\Commands\CompletarNegociacion\CompletarNegociacionHandler.cs**

**Cambios:**
- Validaci�n de existencia de `TipoDocumento` si se proporciona
- Validaci�n de existencia de `Banco` si se proporciona
- Asignaci�n de `IdTipoDocumento` e `IdBanco` a la negociaci�n
- Carga de relaciones `TipoDocumento` y `Banco` en el DTO de respuesta

```csharp
// Validar que el tipo de documento existe si se proporciona
if (dto.IdTipoDocumento.HasValue)
{
    var tipoDocumento = await _unitOfWork.Repository<TipoDocumento>()
        .GetByIdAsync(dto.IdTipoDocumento.Value, cancellationToken);
    
    if (tipoDocumento == null)
        throw new NotFoundException("TipoDocumento", dto.IdTipoDocumento.Value);
}

// Validar que el banco existe si se proporciona
if (dto.IdBanco.HasValue)
{
    var banco = await _unitOfWork.Repository<Banco>()
        .GetByIdAsync(dto.IdBanco.Value, cancellationToken);
    
    if (banco == null)
        throw new NotFoundException("Banco", dto.IdBanco.Value);
}

// Actualizar la negociaci�n
negociacion.IdTipoDocumento = dto.IdTipoDocumento;
negociacion.IdBanco = dto.IdBanco;
```

### 3. Validator (1 archivo)
? **Miski.Application\Features\Compras\Negociaciones\Commands\CompletarNegociacion\CompletarNegociacionValidator.cs**

**Cambios:**
- Validaci�n condicional para `IdTipoDocumento` (si se proporciona, debe ser > 0)
- Validaci�n condicional para `IdBanco` (si se proporciona, debe ser > 0)

```csharp
// Validaci�n condicional para IdTipoDocumento si se proporciona
When(x => x.Completar.IdTipoDocumento.HasValue, () =>
{
    RuleFor(x => x.Completar.IdTipoDocumento!.Value)
        .GreaterThan(0)
        .WithMessage("Debe seleccionar un tipo de documento v�lido");
});

// Validaci�n condicional para IdBanco si se proporciona
When(x => x.Completar.IdBanco.HasValue, () =>
{
    RuleFor(x => x.Completar.IdBanco!.Value)
        .GreaterThan(0)
        .WithMessage("Debe seleccionar un banco v�lido");
});
```

### 4. MappingProfile (1 archivo)
? **Miski.Application\Mappings\MappingProfile.cs**

**Cambios:**
- Mapeo de `TipoDocumentoNombre` desde la relaci�n `TipoDocumento`
- Mapeo de `BancoNombre` desde la relaci�n `Banco`

```csharp
CreateMap<Negociacion, NegociacionDto>()
    // ...mapeos existentes...
    .ForMember(dest => dest.TipoDocumentoNombre, opt => opt.MapFrom(src => 
        src.TipoDocumento != null ? src.TipoDocumento.Nombre : string.Empty))
    .ForMember(dest => dest.BancoNombre, opt => opt.MapFrom(src => 
        src.Banco != null ? src.Banco.Nombre : string.Empty));
```

### 5. Queries (2 archivos)
? **GetNegociacionesHandler.cs** - Carga de relaciones TipoDocumento y Banco
? **GetNegociacionByIdHandler.cs** - Carga de relaciones TipoDocumento y Banco

### 6. Controller (1 archivo)
? **NegociacionesController.cs** - Documentaci�n actualizada

---

## ?? Endpoint Actualizado

### PUT /api/compras/negociaciones/{id}/completar

**Content-Type:** `multipart/form-data`

#### Request Body:

| Campo | Tipo | Requerido | Descripci�n |
|-------|------|-----------|-------------|
| `IdNegociacion` | int | ? S� | ID de la negociaci�n |
| `IdTipoDocumento` | int? | ? No | Tipo de documento del proveedor |
| `IdBanco` | int? | ? No | Banco de la cuenta bancaria |
| `NroDocumentoProveedor` | string | ? S� | N�mero de documento (8-20 caracteres) |
| `NroCuentaBancaria` | string | ? S� | N�mero de cuenta (10-30 caracteres) |
| `FotoDniFrontal` | file | ? S� | Foto frontal del DNI |
| `FotoDniPosterior` | file | ? S� | Foto posterior del DNI |
| `PrimeraEvindenciaFoto` | file | ? S� | Primera evidencia fotogr�fica |
| `SegundaEvindenciaFoto` | file | ? S� | Segunda evidencia fotogr�fica |
| `TerceraEvindenciaFoto` | file | ? S� | Tercera evidencia fotogr�fica |
| `EvidenciaVideo` | file | ? S� | Evidencia en video |

---

## ?? Ejemplo de Uso

### Request (usando FormData):

```javascript
const formData = new FormData();
formData.append('IdNegociacion', '1');
formData.append('IdTipoDocumento', '1');  // DNI
formData.append('IdBanco', '2');  // BCP
formData.append('NroDocumentoProveedor', '12345678');
formData.append('NroCuentaBancaria', '1234567890123456');
formData.append('FotoDniFrontal', dniFrontalFile);
formData.append('FotoDniPosterior', dniPosteriorFile);
formData.append('PrimeraEvindenciaFoto', evidencia1File);
formData.append('SegundaEvindenciaFoto', evidencia2File);
formData.append('TerceraEvindenciaFoto', evidencia3File);
formData.append('EvidenciaVideo', videoFile);

fetch('/api/compras/negociaciones/1/completar', {
    method: 'PUT',
    body: formData
});
```

### Response:

```json
{
  "success": true,
  "message": "Negociaci�n completada exitosamente. Estado cambiado a 'EN REVISI�N'",
  "data": {
    "idNegociacion": 1,
    "idComisionista": 5,
    "idTipoDocumento": 1,
    "idBanco": 2,
    "nroDocumentoProveedor": "12345678",
    "nroCuentaBancaria": "1234567890123456",
    "fotoDniFrontal": "negociaciones/dni/xxx.jpg",
    "fotoDniPosterior": "negociaciones/dni/yyy.jpg",
    "primeraEvindenciaFoto": "negociaciones/evidencias/aaa.jpg",
    "segundaEvindenciaFoto": "negociaciones/evidencias/bbb.jpg",
    "terceraEvindenciaFoto": "negociaciones/evidencias/ccc.jpg",
    "evidenciaVideo": "negociaciones/videos/zzz.mp4",
    "estado": "EN REVISI�N",
    "estadoAprobacionContadora": "PENDIENTE",
    "tipoDocumentoNombre": "DNI",
    "bancoNombre": "BCP"
  }
}
```

---

## ? Validaciones

### Si IdTipoDocumento NO se proporciona:
- ? Contin�a normalmente
- ? El campo `IdTipoDocumento` queda como `null`
- ? El campo `TipoDocumentoNombre` en el DTO ser� `string.Empty`

### Si IdTipoDocumento S� se proporciona:
- ? Valida que sea mayor que 0
- ? Valida que el tipo de documento exista en la base de datos
- ? Si no existe, lanza `NotFoundException`
- ? Carga la relaci�n `TipoDocumento` en el DTO de respuesta

### Si IdBanco NO se proporciona:
- ? Contin�a normalmente
- ? El campo `IdBanco` queda como `null`
- ? El campo `BancoNombre` en el DTO ser� `string.Empty`

### Si IdBanco S� se proporciona:
- ? Valida que sea mayor que 0
- ? Valida que el banco exista en la base de datos
- ? Si no existe, lanza `NotFoundException`
- ? Carga la relaci�n `Banco` en el DTO de respuesta

---

## ?? Flujo de Estados

```
NEGOCIACI�N CREADA
    ?
[EN PROCESO] ? Crear Negociaci�n
    ?
[APROBADO] ? Aprobar por Ingeniero
    ?
[EN REVISI�N] ? Completar Negociaci�n ? (AQU� SE USAN LOS NUEVOS CAMPOS)
    ?
[FINALIZADO] ? Aprobar por Contadora
```

---

## ?? Relaciones Agregadas

### En la Entidad Negociacion:

```csharp
public class Negociacion
{
    // ...campos existentes...
    public int? IdTipoDocumento { get; set; }
    public int? IdBanco { get; set; }
    
    // Navigation properties
    public virtual TipoDocumento? TipoDocumento { get; set; }
    public virtual Banco? Banco { get; set; }
}
```

### En el DbContext:

```csharp
entity.HasOne(d => d.TipoDocumento)
    .WithMany(p => p.Negociaciones)
    .HasForeignKey(d => d.IdTipoDocumento)
    .IsRequired(false)
    .OnDelete(DeleteBehavior.Restrict)
    .HasConstraintName("FK_Negociacion_TipoDocumento");

entity.HasOne(d => d.Banco)
    .WithMany(p => p.Negociaciones)
    .HasForeignKey(d => d.IdBanco)
    .IsRequired(false)
    .OnDelete(DeleteBehavior.Restrict)
    .HasConstraintName("FK_Negociacion_Banco");
```

---

## ?? Casos de Uso

### 1. Completar con DNI y Banco BCP:
```
IdTipoDocumento: 1 (DNI)
IdBanco: 1 (BCP)
NroDocumentoProveedor: "12345678"
NroCuentaBancaria: "1234567890123456"
+ archivos...
```

### 2. Completar con RUC y Banco Interbank:
```
IdTipoDocumento: 2 (RUC)
IdBanco: 2 (Interbank)
NroDocumentoProveedor: "20123456789"
NroCuentaBancaria: "9876543210987654"
+ archivos...
```

### 3. Completar sin especificar tipo de documento ni banco:
```
IdTipoDocumento: null
IdBanco: null
NroDocumentoProveedor: "12345678"
NroCuentaBancaria: "1234567890123456"
+ archivos...
```

---

## ? Estado Final

### Compilaci�n: ? EXITOSA

### Archivos Modificados: 7
- ? NegociacionDto.cs
- ? CompletarNegociacionHandler.cs
- ? CompletarNegociacionValidator.cs
- ? MappingProfile.cs
- ? GetNegociacionesHandler.cs
- ? GetNegociacionByIdHandler.cs
- ? NegociacionesController.cs

### Funcionalidad: ? COMPLETA
- ? Campos opcionales agregados
- ? Validaciones implementadas
- ? Relaciones cargadas
- ? Mapeos actualizados
- ? Documentaci�n actualizada

---

**Fecha de actualizaci�n:** 2024-01-15  
**Estado:** Completado y probado ?  
**Compatibilidad:** ? Retrocompatible (campos opcionales)
