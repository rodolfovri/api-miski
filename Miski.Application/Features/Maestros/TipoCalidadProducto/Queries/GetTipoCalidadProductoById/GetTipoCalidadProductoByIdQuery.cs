using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Queries.GetTipoCalidadProductoById;

public record GetTipoCalidadProductoByIdQuery(int Id) : IRequest<TipoCalidadProductoDto>;
