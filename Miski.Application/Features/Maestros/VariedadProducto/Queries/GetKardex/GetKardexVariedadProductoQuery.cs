using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.VariedadProducto.Queries.GetKardex;

public record GetKardexVariedadProductoQuery(
    int IdVariedadProducto,
    DateTime FechaDesde,
    DateTime FechaHasta,
    string? TipoStock = null
) : IRequest<KardexVariedadProductoDto>;
