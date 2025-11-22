using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Rol.Commands.CreateRol;

public record CreateRolCommand(CreateRolDto Rol) : IRequest<RolMaestroDto>;
