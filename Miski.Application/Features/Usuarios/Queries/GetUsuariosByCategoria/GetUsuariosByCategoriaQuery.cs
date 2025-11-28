using MediatR;
using Miski.Shared.DTOs.Usuarios;

namespace Miski.Application.Features.Usuarios.Queries.GetUsuariosByCategoria;

public record GetUsuariosByCategoriaQuery(
    int IdCategoria,
    string? Estado = null
) : IRequest<List<UsuarioDto>>;
