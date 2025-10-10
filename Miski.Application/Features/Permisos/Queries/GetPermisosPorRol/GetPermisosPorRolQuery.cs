using MediatR;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Queries.GetPermisosPorRol;

public record GetPermisosPorRolQuery(int IdRol) : IRequest<PermisoRolConJerarquiaDto>;