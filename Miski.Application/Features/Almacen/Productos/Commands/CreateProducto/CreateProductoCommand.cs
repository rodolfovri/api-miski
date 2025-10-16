using MediatR;
using Miski.Shared.DTOs.Almacen;

namespace Miski.Application.Features.Almacen.Productos.Commands.CreateProducto;

public record CreateProductoCommand(CreateProductoDto Producto) : IRequest<ProductoDto>;
