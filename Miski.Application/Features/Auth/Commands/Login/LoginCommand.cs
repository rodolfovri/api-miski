using MediatR;
using Miski.Shared.DTOs.Auth;

namespace Miski.Application.Features.Auth.Commands.Login;

public record LoginCommand(LoginDto LoginData) : IRequest<AuthResponseDto>;