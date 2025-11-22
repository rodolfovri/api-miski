using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Rol.Queries.GetRolById;

public record GetRolByIdQuery(int Id) : IRequest<RolMaestroDto>;
