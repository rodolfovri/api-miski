using MediatR;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Commands.DeleteTipoCalidadProducto;

public record DeleteTipoCalidadProductoCommand(int Id) : IRequest<Unit>;
