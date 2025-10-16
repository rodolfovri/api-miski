using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Queries.GetCategorias;

public record GetCategoriasProductoQuery(
    string? Nombre = null,
    string? Estado = null
) : IRequest<List<CategoriaProductoDto>>;