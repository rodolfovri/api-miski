using MediatR;
using Miski.Shared.DTOs.Almacen;

namespace Miski.Application.Features.Almacen.Productos.Queries.GetProductos;

public record GetProductosQuery(
    string? Nombre = null, 
    int? IdCategoriaProducto = null, 
    string? Estado = null
) : IRequest<List<ProductoDto>>;
