using MediatR;
using Miski.Shared.DTOs.Auth;

namespace Miski.Application.Features.Auth.Commands.ChangePassword;

public record ChangePasswordCommand(int UserId, ChangePasswordDto ChangePasswordData) : IRequest<bool>;