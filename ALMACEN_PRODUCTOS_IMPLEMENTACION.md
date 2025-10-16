# Módulo de Almacén - Productos

## ? Estructura Implementada

Se ha creado el módulo completo de **Almacén/Productos** siguiendo la arquitectura Clean Architecture del proyecto.

### ?? Estructura de Archivos Creados:

```
Miski.Api/
??? Controllers/
    ??? Almacen/
        ??? ProductosController.cs          ? Controlador principal
        ??? README.md                        ? Documentación del módulo

Miski.Application/
??? Features/
    ??? Almacen/
        ??? Productos/
            ??? Commands/
            ?   ??? CreateProducto/
            ?   ?   ??? CreateProductoCommand.cs     ? (record)
            ?   ?   ??? CreateProductoHandler.cs     ?
            ?   ??? UpdateProducto/
            ?   ?   ??? UpdateProductoCommand.cs     ? (record)
            ?   ?   ??? UpdateProductoHandler.cs     ?
            ?   ??? DeleteProducto/
            ?       ??? DeleteProductoCommand.cs     ? (record)
            ?       ??? DeleteProductoHandler.cs     ?
            ??? Queries/
                ??? GetProductos/
                ?   ??? GetProductosQuery.cs         ? (record)
                ?   ??? GetProductosHandler.cs       ?
                ??? GetProductoById/
                    ??? GetProductoByIdQuery.cs      ? (record)
                    ??? GetProductoByIdHandler.cs    ?

Miski.Shared/
??? DTOs/
    ??? Almacen/
        ??? ProductoDto.cs                   ? (ProductoDto, CreateProductoDto, UpdateProductoDto)

Miski.Application/
??? Mappings/
    ??? MappingProfile.cs                    ? Mapeo agregado para Producto
```

---

## ?? Funcionalidades Implementadas

### 1. **Queries (Consultas)** - usando `record`

#### GetProductosQuery
```csharp
public record GetProductosQuery(
    string? Nombre = null, 
    string? Codigo = null, 
    int? IdCategoriaProducto = null, 
    string? Estado = null
) : IRequest<List<ProductoDto>>;
```
- Obtiene listado de productos con filtros opcionales
- Filtros: `nombre`, `codigo`, `idCategoriaProducto`, `estado`
- Carga automática de relaciones: `CategoriaProducto` y `UnidadMedida`
- Endpoint: `GET /api/almacen/productos`

#### GetProductoByIdQuery
```csharp
public record GetProductoByIdQuery(int Id) : IRequest<ProductoDto>;
```
- Obtiene un producto específico por ID
- Incluye información de categoría y unidad de medida
- Endpoint: `GET /api/almacen/productos/{id}`

### 2. **Commands (Comandos)** - usando `record`

#### CreateProductoCommand
```csharp
public record CreateProductoCommand(CreateProductoDto Producto) : IRequest<ProductoDto>;
```
- Crea un nuevo producto
- **Validaciones:**
  - ? Categoría del producto debe existir
  - ? Unidad de medida debe existir
  - ? Código no puede estar duplicado
- Endpoint: `POST /api/almacen/productos`
- **Controlador:** Pasa el DTO completo al Command
  ```csharp
  var command = new CreateProductoCommand(request);
  ```

#### UpdateProductoCommand
```csharp
public record UpdateProductoCommand(int Id, UpdateProductoDto Producto) : IRequest<ProductoDto>;
```
- Actualiza un producto existente
- **Validaciones:**
  - ? Producto debe existir
  - ? Categoría del producto debe existir
  - ? Unidad de medida debe existir
  - ? Código no puede estar duplicado (excepto el mismo producto)
- Endpoint: `PUT /api/almacen/productos/{id}`
- **Controlador:** Pasa el Id y el DTO
  ```csharp
  var command = new UpdateProductoCommand(id, request);
  ```

#### DeleteProductoCommand
```csharp
public record DeleteProductoCommand(int Id) : IRequest<Unit>;
```
- Elimina un producto
- **Validaciones:**
  - ? Producto debe existir
  - ? No puede tener stock asociado
  - ? No puede tener negociaciones asociadas
- Endpoint: `DELETE /api/almacen/productos/{id}`

---

## ?? DTOs Creados

### ProductoDto
```csharp
{
    IdProducto,
    IdCategoriaProducto,
    IdUnidadMedida,
    Codigo,
    Nombre,
    Descripcion,
    Estado,
    FRegistro,
    CategoriaProductoNombre,    // Calculado
    UnidadMedidaNombre          // Calculado
}
```

### CreateProductoDto
```csharp
{
    IdCategoriaProducto,
    IdUnidadMedida,
    Codigo,
    Nombre,
    Descripcion,
    Estado (default: "ACTIVO")
}
```

### UpdateProductoDto
```csharp
{
    IdProducto,
    IdCategoriaProducto,
    IdUnidadMedida,
    Codigo,
    Nombre,
    Descripcion,
    Estado
}
```

---

## ?? Características de Arquitectura

? **Clean Architecture**
- Separación clara de responsabilidades por capas
- Dependencias apuntando hacia el dominio

? **CQRS Pattern**
- Commands y Queries completamente separados
- **Uso de `record` types** para inmutabilidad
- Handlers especializados para cada operación

? **MediatR**
- Desacoplamiento total entre controlador y lógica de negocio
- Commands reciben DTOs completos (no propiedades individuales)
- Código más mantenible y testeable

? **Repository Pattern + Unit of Work**
- Abstracción completa del acceso a datos
- Manejo transaccional consistente

? **AutoMapper**
- Mapeo automático de entidades a DTOs
- Configuración centralizada en MappingProfile

? **Manejo de Excepciones**
- `NotFoundException`: Cuando no se encuentra una entidad
- `ValidationException`: Para errores de validación de negocio
- Respuestas HTTP consistentes con ApiResponse

? **Validaciones de Negocio**
- Validación de existencia de entidades relacionadas
- Prevención de duplicados
- Validación de integridad referencial

---

## ?? Endpoints Disponibles

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/almacen/productos` | Lista todos los productos (con filtros) |
| GET | `/api/almacen/productos/{id}` | Obtiene un producto por ID |
| POST | `/api/almacen/productos` | Crea un nuevo producto |
| PUT | `/api/almacen/productos/{id}` | Actualiza un producto |
| DELETE | `/api/almacen/productos/{id}` | Elimina un producto |

---

## ?? Ejemplos de Uso

### Crear Producto
```http
POST /api/almacen/productos
Content-Type: application/json

{
  "idCategoriaProducto": 1,
  "idUnidadMedida": 2,
  "codigo": "PROD-001",
  "nombre": "Café Arábica",
  "descripcion": "Café de alta calidad",
  "estado": "ACTIVO"
}
```

### Actualizar Producto
```http
PUT /api/almacen/productos/1
Content-Type: application/json

{
  "idProducto": 1,
  "idCategoriaProducto": 1,
  "idUnidadMedida": 2,
  "codigo": "PROD-001",
  "nombre": "Café Arábica Premium",
  "descripcion": "Café de alta calidad premium",
  "estado": "ACTIVO"
}
```

### Consultar Productos con Filtros
```http
GET /api/almacen/productos?nombre=café&estado=ACTIVO&idCategoriaProducto=1
```

---

## ? Estado de Compilación

**Compilación: EXITOSA** ?

Todos los archivos compilaron correctamente sin errores.

---

## ?? Entidades Relacionadas

- **Producto** ? Entidad principal
- **CategoriaProducto** ? Categorización de productos
- **UnidadMedida** ? Unidad de medida del producto
- **Stock** ? Inventario por ubicación
- **Negociacion** ? Negociaciones de compra

---

## ?? Patrón de Implementación

### Commands y Queries con Records
```csharp
// Query con parámetros opcionales
public record GetProductosQuery(
    string? Nombre = null, 
    string? Codigo = null
) : IRequest<List<ProductoDto>>;

// Query simple con ID
public record GetProductoByIdQuery(int Id) : IRequest<ProductoDto>;

// Command con DTO completo
public record CreateProductoCommand(CreateProductoDto Producto) : IRequest<ProductoDto>;

// Command con ID y DTO
public record UpdateProductoCommand(int Id, UpdateProductoDto Producto) : IRequest<ProductoDto>;

// Command solo con ID
public record DeleteProductoCommand(int Id) : IRequest<Unit>;
```

### Uso en el Controlador
```csharp
// Crear: Pasar DTO completo
var command = new CreateProductoCommand(request);

// Actualizar: Pasar ID y DTO
var command = new UpdateProductoCommand(id, request);

// Eliminar: Solo ID
var command = new DeleteProductoCommand(id);

// Query con filtros
var query = new GetProductosQuery(nombre, codigo, idCategoria, estado);

// Query por ID
var query = new GetProductoByIdQuery(id);
```

---

## ?? Documentación Adicional

Consulta el archivo `Miski.Api/Controllers/Almacen/README.md` para más detalles sobre el módulo.
