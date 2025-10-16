using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Commands.CreateCategoria;

public record CreateCategoriaProductoCommand(CreateCategoriaProductoDto Categoria) : IRequest<CategoriaProductoDto>;