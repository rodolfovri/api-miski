using MediatR;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Commands.AsignarAccionSubModuloDetalle;

public class AsignarAccionSubModuloDetalleCommand : IRequest<SubModuloDetalleAccionDto>
{
    public int IdSubModuloDetalle { get; set; }
    public int IdAccion { get; set; }
    public bool Habilitado { get; set; } = true;

    public AsignarAccionSubModuloDetalleCommand(int idSubModuloDetalle, int idAccion, bool habilitado = true)
    {
        IdSubModuloDetalle = idSubModuloDetalle;
        IdAccion = idAccion;
        Habilitado = habilitado;
    }
}
