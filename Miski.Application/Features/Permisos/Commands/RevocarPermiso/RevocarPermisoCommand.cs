using MediatR;

namespace Miski.Application.Features.Permisos.Commands.RevocarPermiso;

public class RevocarPermisoCommand : IRequest<bool>
{
    public int IdRol { get; set; }
    public int? IdModulo { get; set; }
    public int? IdSubModulo { get; set; }
    public int? IdSubModuloDetalle { get; set; }

    public RevocarPermisoCommand(int idRol, int? idModulo = null, int? idSubModulo = null, int? idSubModuloDetalle = null)
    {
        IdRol = idRol;
        IdModulo = idModulo;
        IdSubModulo = idSubModulo;
        IdSubModuloDetalle = idSubModuloDetalle;
    }
}
