using MediatR;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Commands.AsignarPermiso;

public record AsignarPermisoCommand(AsignarPermisoDto Permiso) : IRequest<PermisoRolDto>;