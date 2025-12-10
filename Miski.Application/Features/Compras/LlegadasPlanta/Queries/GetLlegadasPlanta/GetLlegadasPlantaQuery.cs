using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetLlegadasPlanta;

public record GetLlegadasPlantaQuery(
    string? Estado = null,
    DateTime? FechaDesde = null,
    DateTime? FechaHasta = null
) : IRequest<List<LlegadaPlantaDto>>;
