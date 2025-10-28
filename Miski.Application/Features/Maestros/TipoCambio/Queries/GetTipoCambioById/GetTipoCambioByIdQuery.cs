using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCambio.Queries.GetTipoCambioById;

public record GetTipoCambioByIdQuery(int Id) : IRequest<TipoCambioDto>;
