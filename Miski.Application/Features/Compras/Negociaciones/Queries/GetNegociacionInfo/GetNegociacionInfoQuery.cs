using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociacionInfo;

public record GetNegociacionInfoQuery(int IdNegociacion) : IRequest<NegociacionInfoDto>;
