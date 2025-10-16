using MediatR;
using Miski.Shared.DTOs.Almacen;

namespace Miski.Application.Features.Almacen.Productos.Commands.UpdateProducto;

public record UpdateProductoCommand(int Id, UpdateProductoDto Producto) : IRequest<ProductoDto>;
