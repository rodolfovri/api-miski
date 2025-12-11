using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetVehiculosConComprasYRecepciones;

public record GetVehiculosConComprasYRecepcionesQuery(
    DateTime? FechaDesde = null,
    DateTime? FechaHasta = null,
    int? IdVehiculo = null
) : IRequest<List<VehiculoConComprasYRecepcionesDto>>;
