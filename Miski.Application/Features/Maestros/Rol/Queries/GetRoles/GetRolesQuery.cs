using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Rol.Queries.GetRoles;

public record GetRolesQuery(string? TipoPlataforma = null, string? Estado = null) : IRequest<List<RolMaestroDto>>;
