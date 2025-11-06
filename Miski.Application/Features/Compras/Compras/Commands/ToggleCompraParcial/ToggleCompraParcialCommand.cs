using MediatR;

namespace Miski.Application.Features.Compras.Compras.Commands.ToggleCompraParcial;

public record ToggleCompraParcialCommand(int IdCompra) : IRequest<Unit>;
