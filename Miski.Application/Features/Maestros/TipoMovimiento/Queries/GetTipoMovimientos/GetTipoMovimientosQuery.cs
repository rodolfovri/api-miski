using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Queries.GetTipoMovimientos;

public class GetTipoMovimientosQuery : IRequest<List<TipoMovimientoDto>>
{
}
