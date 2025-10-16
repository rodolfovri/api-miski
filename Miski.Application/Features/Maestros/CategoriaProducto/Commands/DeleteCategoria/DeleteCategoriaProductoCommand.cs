using MediatR;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Commands.DeleteCategoria;

public record DeleteCategoriaProductoCommand(int Id) : IRequest<bool>;