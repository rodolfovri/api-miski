using MediatR;
using Miski.Shared.DTOs.Usuarios;

namespace Miski.Application.Features.Usuarios.Commands.UpdateUsuario;

public record UpdateUsuarioCommand(int Id, UpdateUsuarioDto Usuario) : IRequest<UsuarioDto>;
