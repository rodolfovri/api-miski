using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Queries.GetTipoMovimientoById;

public class GetTipoMovimientoByIdQuery : IRequest<TipoMovimientoDto>
{
    public int IdTipoMovimiento { get; set; }
}
