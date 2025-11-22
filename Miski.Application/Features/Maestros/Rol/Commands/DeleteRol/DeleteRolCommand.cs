using MediatR;

namespace Miski.Application.Features.Maestros.Rol.Commands.DeleteRol;

public record DeleteRolCommand(int Id) : IRequest<Unit>;
