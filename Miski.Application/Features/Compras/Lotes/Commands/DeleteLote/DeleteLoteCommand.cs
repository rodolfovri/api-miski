using MediatR;

namespace Miski.Application.Features.Compras.Lotes.Commands.DeleteLote;

public record DeleteLoteCommand(int Id) : IRequest<Unit>;
