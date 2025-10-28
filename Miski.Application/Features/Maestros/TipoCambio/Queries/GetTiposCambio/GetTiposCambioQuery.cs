using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCambio.Queries.GetTiposCambio;

public record GetTiposCambioQuery(
    int? IdMoneda = null,
    DateTime? FechaDesde = null,
    DateTime? FechaHasta = null
) : IRequest<IEnumerable<TipoCambioDto>>;
