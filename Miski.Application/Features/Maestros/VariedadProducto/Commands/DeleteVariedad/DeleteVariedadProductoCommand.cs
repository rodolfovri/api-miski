using MediatR;

namespace Miski.Application.Features.Maestros.VariedadProducto.Commands.DeleteVariedad;

public record DeleteVariedadProductoCommand(int Id) : IRequest<Unit>;
