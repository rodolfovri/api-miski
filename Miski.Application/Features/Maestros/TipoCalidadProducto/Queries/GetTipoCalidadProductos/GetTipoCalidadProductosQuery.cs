using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Queries.GetTipoCalidadProductos;

public record GetTipoCalidadProductosQuery(int? IdProducto = null, string? Estado = null) : IRequest<List<TipoCalidadProductoDto>>;
