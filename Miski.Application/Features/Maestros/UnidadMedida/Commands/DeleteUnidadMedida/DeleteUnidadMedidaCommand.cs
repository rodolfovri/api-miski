using MediatR;

namespace Miski.Application.Features.Maestros.UnidadMedida.Commands.DeleteUnidadMedida;

public record DeleteUnidadMedidaCommand(int Id) : IRequest<bool>;