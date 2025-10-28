using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Queries.GetTipoCalidadByProductoId;

public record GetTipoCalidadByProductoIdQuery(int IdProducto) : IRequest<List<TipoCalidadProductoDto>>;
