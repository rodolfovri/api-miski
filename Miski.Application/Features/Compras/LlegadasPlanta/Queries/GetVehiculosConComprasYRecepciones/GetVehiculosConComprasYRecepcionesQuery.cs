using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetVehiculosConComprasYRecepciones;

public record GetVehiculosConComprasYRecepcionesQuery(
    int? IdCompraVehiculo = null,
    int? IdVehiculo = null
) : IRequest<List<VehiculoConComprasYRecepcionesDto>>;
