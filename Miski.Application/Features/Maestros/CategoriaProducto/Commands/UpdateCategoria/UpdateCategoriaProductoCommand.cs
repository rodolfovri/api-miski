using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Commands.UpdateCategoria;

public record UpdateCategoriaProductoCommand(int Id, UpdateCategoriaProductoDto Categoria) : IRequest<CategoriaProductoDto>;