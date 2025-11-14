using MediatR;
using Miski.Shared.DTOs.Usuarios;

namespace Miski.Application.Features.Usuarios.Queries.GetUsuarios;

public record GetUsuariosQuery(
    string? Estado = null,
    int? IdPersona = null
) : IRequest<List<UsuarioDto>>;
