using MediatR;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraPagos.Commands.DefinirTipoPago;

public record DefinirTipoPagoCommand(
    int IdCompra,
    string TipoPago,
    int? DiasCredito = null,
    decimal? MontoAdelanto = null,
    DateTime? FechaPago = null,
    string? Observacion = null
) : IRequest<ApiResponse<CompraPagoDto>>;
