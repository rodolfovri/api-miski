using MediatR;
using Miski.Shared.DTOs.Usuarios;

namespace Miski.Application.Features.Usuarios.Commands.UpdatePassword;

public record UpdatePasswordCommand(int Id, UpdatePasswordDto Password) : IRequest;
