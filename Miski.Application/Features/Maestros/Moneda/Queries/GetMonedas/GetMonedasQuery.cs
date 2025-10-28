using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Moneda.Queries.GetMonedas;

public record GetMonedasQuery : IRequest<IEnumerable<MonedaDto>>;
