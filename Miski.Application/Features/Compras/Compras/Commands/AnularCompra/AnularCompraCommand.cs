using MediatR;

namespace Miski.Application.Features.Compras.Compras.Commands.AnularCompra;

public record AnularCompraCommand(int IdCompra, int IdUsuarioAnulacion, string MotivoAnulacion) : IRequest<Unit>;
