using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Banco.Queries.GetBancos;

public record GetBancosQuery(string? Estado = null) : IRequest<List<BancoDto>>;
