using MediatR;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Commands.DeleteTipoMovimiento;

public class DeleteTipoMovimientoCommand : IRequest<bool>
{
    public int IdTipoMovimiento { get; set; }
}
