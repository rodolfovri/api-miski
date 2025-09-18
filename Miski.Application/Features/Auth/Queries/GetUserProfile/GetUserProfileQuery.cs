using MediatR;
using Miski.Shared.DTOs.Auth;

namespace Miski.Application.Features.Auth.Queries.GetUserProfile;

public record GetUserProfileQuery(int UserId) : IRequest<AuthResponseDto>;