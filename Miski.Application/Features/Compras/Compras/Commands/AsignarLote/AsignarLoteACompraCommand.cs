using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Compras.Commands.AsignarLote;

public record AsignarLoteACompraCommand(int IdCompra, int IdLote, decimal MontoTotal) : IRequest<CompraDto>;
