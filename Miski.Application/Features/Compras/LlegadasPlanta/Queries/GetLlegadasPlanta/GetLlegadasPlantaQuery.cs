using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetLlegadasPlanta;

public record GetLlegadasPlantaQuery(
    int? IdCompra = null,
    string? Estado = null,
    DateTime? FechaInicio = null,
    DateTime? FechaFin = null
) : IRequest<List<LlegadaPlantaDto>>;
