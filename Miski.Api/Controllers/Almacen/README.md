# Módulo de Almacén

Este módulo gestiona todo lo relacionado con el almacenamiento y control de productos.

## Funcionalidades principales:
- CRUD de productos
- Gestión de stock por producto y ubicación
- Control de inventario
- Movimientos de almacén

## Controladores:
- `ProductosController`: CRUD completo de productos con validaciones

## Estructura del módulo:

### Controllers (API Layer)
```
Miski.Api/Controllers/Almacen/
??? ProductosController.cs
```

### Features (Application Layer)
```
Miski.Application/Features/Almacen/Productos/
??? Commands/
?   ??? CreateProducto/
?   ?   ??? CreateProductoCommand.cs
?   ?   ??? CreateProductoHandler.cs
?   ??? UpdateProducto/
?   ?   ??? UpdateProductoCommand.cs
?   ?   ??? UpdateProductoHandler.cs
?   ??? DeleteProducto/
?       ??? DeleteProductoCommand.cs
?       ??? DeleteProductoHandler.cs
??? Queries/
    ??? GetProductos/
    ?   ??? GetProductosQuery.cs
    ?   ??? GetProductosHandler.cs
    ??? GetProductoById/
        ??? GetProductoByIdQuery.cs
        ??? GetProductoByIdHandler.cs
```

### DTOs (Shared Layer)
```
Miski.Shared/DTOs/Almacen/
??? ProductoDto.cs
    ??? ProductoDto
    ??? CreateProductoDto
    ??? UpdateProductoDto
```

## Endpoints Disponibles:

### 1. Obtener todos los productos
```http
GET /api/almacen/productos
Query params: ?nombre=xxx&codigo=xxx&idCategoriaProducto=1&estado=ACTIVO
```

### 2. Obtener producto por ID
```http
GET /api/almacen/productos/{id}
```

### 3. Crear producto
```http
POST /api/almacen/productos
Body:
{
  "idCategoriaProducto": 1,
  "idUnidadMedida": 1,
  "codigo": "PROD001",
  "nombre": "Producto de Ejemplo",
  "descripcion": "Descripción del producto",
  "estado": "ACTIVO"
}
```

### 4. Actualizar producto
```http
PUT /api/almacen/productos/{id}
Body:
{
  "idProducto": 1,
  "idCategoriaProducto": 1,
  "idUnidadMedida": 1,
  "codigo": "PROD001",
  "nombre": "Producto Actualizado",
  "descripcion": "Nueva descripción",
  "estado": "ACTIVO"
}
```

### 5. Eliminar producto
```http
DELETE /api/almacen/productos/{id}
```

## Validaciones Implementadas:

### CreateProducto:
- ? Validar que la categoría del producto exista
- ? Validar que la unidad de medida exista
- ? Validar que el código no esté duplicado
- ? Campos requeridos: Código, Nombre, IdCategoriaProducto, IdUnidadMedida

### UpdateProducto:
- ? Validar que el producto exista
- ? Validar que la categoría del producto exista
- ? Validar que la unidad de medida exista
- ? Validar que el código no esté duplicado (excepto el mismo producto)

### DeleteProducto:
- ? Validar que el producto exista
- ? Validar que no tenga stock asociado
- ? Validar que no tenga negociaciones asociadas

## Entidades Relacionadas:
- `Producto`: Entidad principal
- `CategoriaProducto`: Categoría del producto
- `UnidadMedida`: Unidad de medida del producto
- `Stock`: Stock del producto por ubicación
- `Negociacion`: Negociaciones que involucran el producto

## Arquitectura Aplicada:
- ? Clean Architecture
- ? CQRS Pattern (Commands y Queries separadas)
- ? MediatR para desacoplar handlers
- ? Repository Pattern + Unit of Work
- ? AutoMapper para mapeo de DTOs
- ? Manejo de excepciones personalizado

## Prioridad: ALTA
Los productos son fundamentales para todo el sistema de compras y almacenamiento.
