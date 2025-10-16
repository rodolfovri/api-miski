using MediatR;
using Miski.Shared.DTOs.Almacen;

namespace Miski.Application.Features.Almacen.Productos.Queries.GetProductoById;

public record GetProductoByIdQuery(int Id) : IRequest<ProductoDto>;
