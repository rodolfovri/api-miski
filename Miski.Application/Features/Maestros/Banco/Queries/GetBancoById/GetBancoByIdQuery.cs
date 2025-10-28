using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Banco.Queries.GetBancoById;

public record GetBancoByIdQuery(int Id) : IRequest<BancoDto>;
