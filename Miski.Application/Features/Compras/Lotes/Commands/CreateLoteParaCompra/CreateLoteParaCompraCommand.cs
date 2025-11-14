using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Lotes.Commands.CreateLoteParaCompra;

public record CreateLoteParaCompraCommand(int IdCompra, CreateLoteDto Lote, decimal MontoTotal) : IRequest<LoteDto>;
