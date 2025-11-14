using MediatR;

namespace Miski.Application.Features.Usuarios.Commands.DeleteUsuario;

public record DeleteUsuarioCommand(int Id) : IRequest;
