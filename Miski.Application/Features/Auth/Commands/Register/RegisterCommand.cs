using MediatR;
using Miski.Shared.DTOs.Auth;

namespace Miski.Application.Features.Auth.Commands.Register;

public record RegisterCommand(RegisterDto RegisterData) : IRequest<AuthResponseDto>;