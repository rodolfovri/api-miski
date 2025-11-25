using MediatR;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Commands.AsignarPermisosMultiples;

public class AsignarPermisosMultiplesCommand : IRequest<List<PermisoRolDto>>
{
    public AsignarPermisosMultiplesDto Data { get; set; }

    public AsignarPermisosMultiplesCommand(AsignarPermisosMultiplesDto data)
    {
        Data = data;
    }
}
