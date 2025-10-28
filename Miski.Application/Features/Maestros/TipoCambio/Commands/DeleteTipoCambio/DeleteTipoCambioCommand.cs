using MediatR;

namespace Miski.Application.Features.Maestros.TipoCambio.Commands.DeleteTipoCambio;

public record DeleteTipoCambioCommand(int Id) : IRequest<Unit>;
