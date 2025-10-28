using MediatR;

namespace Miski.Application.Features.Maestros.Moneda.Commands.DeleteMoneda;

public record DeleteMonedaCommand(int Id) : IRequest<Unit>;
