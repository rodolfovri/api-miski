using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociacionById;

public record GetNegociacionByIdQuery(int Id) : IRequest<NegociacionDto>;
