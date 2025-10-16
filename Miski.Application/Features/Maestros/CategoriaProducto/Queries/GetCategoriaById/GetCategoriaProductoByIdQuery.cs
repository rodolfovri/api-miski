using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Queries.GetCategoriaById;

public record GetCategoriaProductoByIdQuery(int Id) : IRequest<CategoriaProductoDto>;