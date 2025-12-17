using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Commands.UpdateTipoMovimiento;

public class UpdateTipoMovimientoCommand : IRequest<TipoMovimientoDto>
{
    public UpdateTipoMovimientoDto TipoMovimientoData { get; set; } = null!;
}
