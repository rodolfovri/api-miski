using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Moneda.Queries.GetMonedaById;

public record GetMonedaByIdQuery(int Id) : IRequest<MonedaDto>;
