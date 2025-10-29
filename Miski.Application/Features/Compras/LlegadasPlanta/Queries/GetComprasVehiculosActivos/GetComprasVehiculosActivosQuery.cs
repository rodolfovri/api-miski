using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetComprasVehiculosActivos;

public record GetComprasVehiculosActivosQuery : IRequest<List<CompraVehiculoResumenDto>>;
