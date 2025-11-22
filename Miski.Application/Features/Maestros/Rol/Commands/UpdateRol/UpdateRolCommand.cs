using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Rol.Commands.UpdateRol;

public record UpdateRolCommand(int Id, UpdateRolDto Rol) : IRequest<RolMaestroDto>;
