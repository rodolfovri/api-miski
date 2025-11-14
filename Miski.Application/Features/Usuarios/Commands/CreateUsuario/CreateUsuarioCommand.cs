using MediatR;
using Miski.Shared.DTOs.Usuarios;

namespace Miski.Application.Features.Usuarios.Commands.CreateUsuario;

public record CreateUsuarioCommand(CreateUsuarioDto Usuario) : IRequest<UsuarioDto>;
