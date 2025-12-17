using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Commands.CreateTipoMovimiento;

public class CreateTipoMovimientoCommand : IRequest<TipoMovimientoDto>
{
    public CreateTipoMovimientoDto TipoMovimientoData { get; set; } = null!;
}
