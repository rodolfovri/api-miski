using MediatR;

namespace Miski.Application.Features.Compras.Compras.Commands.DesasignarLote;

public record DesasignarLoteDeCompraCommand(int IdCompra) : IRequest<Unit>;
