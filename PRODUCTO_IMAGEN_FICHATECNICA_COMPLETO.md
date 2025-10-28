# ? PRODUCTO - Imagen y Ficha T�cnica - IMPLEMENTACI�N COMPLETA

## ?? COMPILACI�N EXITOSA

---

## ?? Resumen de la Implementaci�n

Se han agregado los campos `Imagen` y `FichaTecnica` al CRUD de **Producto** para permitir la carga de archivos (im�genes y PDFs).

---

## ?? Archivos Modificados

### 1. DTOs ?
**Archivo:** `Miski.Shared\DTOs\Almacen\ProductoDto.cs`

```csharp
public class ProductoDto
{
    public string? Imagen { get; set; }  // ? URL de la imagen
    public string? FichaTecnica { get; set; }  // ? URL del PDF
}

public class CreateProductoDto
{
    public IFormFile? Imagen { get; set; }  // ? Archivo opcional
    public IFormFile? FichaTecnica { get; set; }  // ? Archivo opcional
}

public class UpdateProductoDto
{
    public IFormFile? Imagen { get; set; }  // ? Archivo opcional
    public IFormFile? FichaTecnica { get; set; }  // ? Archivo opcional
}
```

### 2. CreateProductoHandler ?
**Archivo:** `Miski.Application\Features\Almacen\Productos\Commands\CreateProducto\CreateProductoHandler.cs`

**Cambios:**
- ? Inyecci�n de `IFileStorageService`
- ? Guarda imagen en carpeta `productos/imagenes`
- ? Guarda ficha t�cnica en carpeta `productos/fichas-tecnicas`
- ? Asigna URLs a la entidad Producto

```csharp
// Guardar imagen si se proporciona
string? imagenUrl = null;
if (dto.Imagen != null)
{
    imagenUrl = await _fileStorageService.SaveFileAsync(
        dto.Imagen, 
        "productos/imagenes", 
        cancellationToken);
}

// Guardar ficha t�cnica (PDF) si se proporciona
string? fichaTecnicaUrl = null;
if (dto.FichaTecnica != null)
{
    fichaTecnicaUrl = await _fileStorageService.SaveFileAsync(
        dto.FichaTecnica, 
        "productos/fichas-tecnicas", 
        cancellationToken);
}

var producto = new Producto
{
    // ...otros campos...
    Imagen = imagenUrl,
    FichaTecnica = fichaTecnicaUrl,
    // ...
};
```

### 3. UpdateProductoHandler ?
**Archivo:** `Miski.Application\Features\Almacen\Productos\Commands\UpdateProducto\UpdateProductoHandler.cs`

**Cambios:**
- ? Inyecci�n de `IFileStorageService`
- ? Actualiza imagen (elimina la anterior si existe)
- ? Actualiza ficha t�cnica (elimina la anterior si existe)
- ? Solo actualiza si se proporciona un nuevo archivo

```csharp
// Actualizar imagen si se proporciona una nueva
if (dto.Imagen != null)
{
    // Eliminar imagen anterior si existe
    if (!string.IsNullOrEmpty(producto.Imagen))
    {
        await _fileStorageService.DeleteFileAsync(producto.Imagen, cancellationToken);
    }

    producto.Imagen = await _fileStorageService.SaveFileAsync(
        dto.Imagen, 
        "productos/imagenes", 
        cancellationToken);
}

// Actualizar ficha t�cnica si se proporciona una nueva
if (dto.FichaTecnica != null)
{
    // Eliminar ficha t�cnica anterior si existe
    if (!string.IsNullOrEmpty(producto.FichaTecnica))
    {
        await _fileStorageService.DeleteFileAsync(producto.FichaTecnica, cancellationToken);
    }

    producto.FichaTecnica = await _fileStorageService.SaveFileAsync(
        dto.FichaTecnica, 
        "productos/fichas-tecnicas", 
        cancellationToken);
}
```

### 4. ProductosController ?
**Archivo:** `Miski.Api\Controllers\Almacen\ProductosController.cs`

**Cambios:**
- ? POST acepta `multipart/form-data`
- ? PUT acepta `multipart/form-data`
- ? Documentaci�n actualizada con los nuevos campos

```csharp
[HttpPost]
[Consumes("multipart/form-data")]
public async Task<ActionResult<ApiResponse<ProductoDto>>> CreateProducto(
    [FromForm] CreateProductoDto request,
    CancellationToken cancellationToken = default)

[HttpPut("{id}")]
[Consumes("multipart/form-data")]
public async Task<ActionResult<ApiResponse<ProductoDto>>> UpdateProducto(
    int id,
    [FromForm] UpdateProductoDto request,
    CancellationToken cancellationToken = default)
```

### 5. MappingProfile ?
**Archivo:** `Miski.Application\Mappings\MappingProfile.cs`

```csharp
CreateMap<Producto, ProductoDto>()
    .ForMember(dest => dest.Imagen, opt => opt.MapFrom(src => src.Imagen))
    .ForMember(dest => dest.FichaTecnica, opt => opt.MapFrom(src => src.FichaTecnica));
```

### 6. DbContext ?
**Archivo:** `Miski.Infrastructure\Data\MiskiDbContext.cs`

```csharp
entity.Property(e => e.Imagen).HasMaxLength(255);
entity.Property(e => e.FichaTecnica).HasMaxLength(255);
```

---

## ?? Estructura de Carpetas

```
uploads/
??? productos/
    ??? imagenes/
    ?   ??? abc123.jpg
    ?   ??? def456.png
    ?   ??? ghi789.jpeg
    ??? fichas-tecnicas/
        ??? abc123.pdf
        ??? def456.pdf
        ??? ghi789.pdf
```

---

## ?? Endpoints Actualizados

### 1. POST /api/almacen/productos

**Content-Type:** `multipart/form-data`

**Campos:**
| Campo | Tipo | Requerido | Descripci�n |
|-------|------|-----------|-------------|
| `IdCategoriaProducto` | int | ? S� | ID de la categor�a |
| `Nombre` | string | ? S� | Nombre del producto |
| `Descripcion` | string | ? No | Descripci�n del producto |
| `Estado` | string | ? No | Estado (default: ACTIVO) |
| `Imagen` | file | ? No | Archivo de imagen |
| `FichaTecnica` | file | ? No | Archivo PDF |

**Ejemplo con FormData (JavaScript):**
```javascript
const formData = new FormData();
formData.append('IdCategoriaProducto', '1');
formData.append('Nombre', 'Quinua Blanca');
formData.append('Descripcion', 'Quinua org�nica de alta calidad');
formData.append('Estado', 'ACTIVO');
formData.append('Imagen', imagenFile);  // File object
formData.append('FichaTecnica', pdfFile);  // File object

fetch('/api/almacen/productos', {
    method: 'POST',
    body: formData
});
```

**Response:**
```json
{
  "success": true,
  "message": "Producto creado exitosamente",
  "data": {
    "idProducto": 1,
    "idCategoriaProducto": 1,
    "nombre": "Quinua Blanca",
    "descripcion": "Quinua org�nica de alta calidad",
    "estado": "ACTIVO",
    "imagen": "productos/imagenes/abc123.jpg",
    "fichaTecnica": "productos/fichas-tecnicas/def456.pdf",
    "categoriaProductoNombre": "Granos Andinos"
  }
}
```

### 2. PUT /api/almacen/productos/{id}

**Content-Type:** `multipart/form-data`

**Campos:**
| Campo | Tipo | Requerido | Descripci�n |
|-------|------|-----------|-------------|
| `IdProducto` | int | ? S� | ID del producto |
| `IdCategoriaProducto` | int | ? S� | ID de la categor�a |
| `Nombre` | string | ? S� | Nombre del producto |
| `Descripcion` | string | ? No | Descripci�n del producto |
| `Estado` | string | ? No | Estado del producto |
| `Imagen` | file | ? No | Nueva imagen (reemplaza la anterior) |
| `FichaTecnica` | file | ? No | Nueva ficha t�cnica (reemplaza la anterior) |

**Ejemplo con FormData:**
```javascript
const formData = new FormData();
formData.append('IdProducto', '1');
formData.append('IdCategoriaProducto', '1');
formData.append('Nombre', 'Quinua Blanca Premium');
formData.append('Imagen', nuevaImagenFile);  // Solo si se quiere cambiar

fetch('/api/almacen/productos/1', {
    method: 'PUT',
    body: formData
});
```

**Response:**
```json
{
  "success": true,
  "message": "Producto actualizado exitosamente",
  "data": {
    "idProducto": 1,
    "nombre": "Quinua Blanca Premium",
    "imagen": "productos/imagenes/xyz789.jpg",  // Nueva URL
    "fichaTecnica": "productos/fichas-tecnicas/def456.pdf"  // Se mantiene si no se envi� nueva
  }
}
```

---

## ?? Casos de Uso

### Caso 1: Crear Producto Sin Archivos
```
POST /api/almacen/productos
{
  "idCategoriaProducto": 1,
  "nombre": "Kiwicha",
  "descripcion": "Cereal andino"
}

Resultado:
- imagen: null
- fichaTecnica: null
```

### Caso 2: Crear Producto Solo con Imagen
```
POST /api/almacen/productos
{
  "idCategoriaProducto": 1,
  "nombre": "Quinua",
  "imagen": [archivo.jpg]
}

Resultado:
- imagen: "productos/imagenes/abc123.jpg"
- fichaTecnica: null
```

### Caso 3: Crear Producto con Imagen y Ficha T�cnica
```
POST /api/almacen/productos
{
  "idCategoriaProducto": 1,
  "nombre": "Quinua Real",
  "imagen": [imagen.jpg],
  "fichaTecnica": [ficha.pdf]
}

Resultado:
- imagen: "productos/imagenes/abc123.jpg"
- fichaTecnica: "productos/fichas-tecnicas/def456.pdf"
```

### Caso 4: Actualizar Solo la Imagen
```
PUT /api/almacen/productos/1
{
  "idProducto": 1,
  "nombre": "Quinua Real",
  "imagen": [nueva-imagen.jpg]
}

Proceso:
1. Elimina imagen anterior
2. Guarda nueva imagen
3. Ficha t�cnica se mantiene sin cambios
```

### Caso 5: Actualizar Sin Cambiar Archivos
```
PUT /api/almacen/productos/1
{
  "idProducto": 1,
  "nombre": "Quinua Real Premium"
}

Proceso:
1. Solo actualiza el nombre
2. Imagen y ficha t�cnica se mantienen
```

---

## ? Validaciones

### Campos Opcionales:
- ? `Imagen` puede ser `null` (no requerida)
- ? `FichaTecnica` puede ser `null` (no requerida)
- ? Al actualizar, si no se env�an, se mantienen los valores actuales

### Eliminaci�n Autom�tica:
- ? Al actualizar con un nuevo archivo, se elimina el anterior
- ? Evita acumulaci�n de archivos hu�rfanos

### Formatos Recomendados:
- **Imagen:** jpg, jpeg, png, gif, webp
- **Ficha T�cnica:** pdf

---

## ?? Servicio de Almacenamiento

**IFileStorageService** proporciona:

```csharp
Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken);
Task DeleteFileAsync(string filePath, CancellationToken cancellationToken);
```

**Caracter�sticas:**
- ? Genera nombres �nicos para evitar sobrescritura
- ? Crea carpetas autom�ticamente
- ? Retorna URL relativa del archivo
- ? Maneja errores de I/O

---

## ?? Integraci�n con Frontend

### React/Vue Example:
```javascript
const uploadProducto = async (data) => {
  const formData = new FormData();
  
  // Campos de texto
  formData.append('IdCategoriaProducto', data.categoriaId);
  formData.append('Nombre', data.nombre);
  formData.append('Descripcion', data.descripcion);
  
  // Archivos
  if (data.imagenFile) {
    formData.append('Imagen', data.imagenFile);
  }
  
  if (data.fichaTecnicaFile) {
    formData.append('FichaTecnica', data.fichaTecnicaFile);
  }
  
  const response = await fetch('/api/almacen/productos', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`
    },
    body: formData
  });
  
  return await response.json();
};
```

### Angular Example:
```typescript
uploadProducto(producto: any, imagen: File, fichaTecnica: File): Observable<any> {
  const formData = new FormData();
  
  formData.append('IdCategoriaProducto', producto.idCategoriaProducto);
  formData.append('Nombre', producto.nombre);
  
  if (imagen) {
    formData.append('Imagen', imagen, imagen.name);
  }
  
  if (fichaTecnica) {
    formData.append('FichaTecnica', fichaTecnica, fichaTecnica.name);
  }
  
  return this.http.post('/api/almacen/productos', formData);
}
```

---

## ?? Beneficios de la Implementaci�n

### ? Usuario:
- Puede adjuntar imagen del producto
- Puede subir ficha t�cnica en PDF
- Visualizaci�n de documentos desde la aplicaci�n

### ? Administrador:
- Mejor gesti�n de informaci�n del producto
- Archivos organizados por carpetas
- F�cil actualizaci�n de documentos

### ? Sistema:
- Archivos almacenados localmente
- URLs relativas para f�cil migraci�n
- Eliminaci�n autom�tica de archivos obsoletos
- Sin acumulaci�n de basura en disco

---

## ?? Comparaci�n con Otros M�dulos

| M�dulo | Carpeta Im�genes | Carpeta Documentos | Servicio |
|--------|------------------|---------------------|----------|
| **Producto** | productos/imagenes | productos/fichas-tecnicas | ? IFileStorageService |
| **Negociaci�n** | negociaciones/evidencias | negociaciones/videos | ? IFileStorageService |
| **Ubicaci�n** | - | ubicaciones/comprobantes | ? IFileStorageService |

**Patr�n Consistente:**
- Todos usan `IFileStorageService`
- Estructura de carpetas organizada por m�dulo
- Eliminaci�n autom�tica de archivos antiguos

---

## ? Estado Final

### Compilaci�n: ? EXITOSA

### Archivos Modificados: 6
1. ? ProductoDto.cs
2. ? CreateProductoHandler.cs
3. ? UpdateProductoHandler.cs
4. ? ProductosController.cs
5. ? MappingProfile.cs
6. ? MiskiDbContext.cs (ya estaba actualizado)

### Funcionalidad: ? COMPLETA
- ? Upload de imagen en CREATE
- ? Upload de ficha t�cnica en CREATE
- ? Update de imagen (con eliminaci�n de anterior)
- ? Update de ficha t�cnica (con eliminaci�n de anterior)
- ? Campos opcionales
- ? Mapeos correctos
- ? Endpoints documentados
- ? Validaciones implementadas

---

**Fecha de implementaci�n:** 2024-01-15  
**Estado:** ? Completado y Compilado  
**Patr�n:** Consistente con Negociaciones y Ubicaciones  
**Framework:** .NET 8
