using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.VariedadProducto.Queries.GetVariedades;

public record GetVariedadesProductoQuery(
    string? Nombre = null,
    string? Codigo = null,
    int? IdProducto = null,
    string? Estado = null
) : IRequest<List<VariedadProductoDto>>;
