using MediatR;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.DeleteNegociacion;

public record DeleteNegociacionCommand(int Id) : IRequest<Unit>;
