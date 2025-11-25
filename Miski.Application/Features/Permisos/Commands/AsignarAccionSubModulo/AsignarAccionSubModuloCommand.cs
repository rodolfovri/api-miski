using MediatR;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Commands.AsignarAccionSubModulo;

public class AsignarAccionSubModuloCommand : IRequest<SubModuloAccionDto>
{
    public int IdSubModulo { get; set; }
    public int IdAccion { get; set; }
    public bool Habilitado { get; set; } = true;

    public AsignarAccionSubModuloCommand(int idSubModulo, int idAccion, bool habilitado = true)
    {
        IdSubModulo = idSubModulo;
        IdAccion = idAccion;
        Habilitado = habilitado;
    }
}
