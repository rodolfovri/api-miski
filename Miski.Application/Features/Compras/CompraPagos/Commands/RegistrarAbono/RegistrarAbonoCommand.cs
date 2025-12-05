using MediatR;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraPagos.Commands.RegistrarAbono;

public record RegistrarAbonoCommand(
    int IdCompra,
    decimal MontoAbono,
    DateTime? FechaPago = null,
    string? Observacion = null
) : IRequest<ApiResponse<CompraPagoDto>>;
