using MediatR;
using Miski.Shared.DTOs.Usuarios;

namespace Miski.Application.Features.Usuarios.Queries.GetUsuarioById;

public record GetUsuarioByIdQuery(int Id) : IRequest<UsuarioDto>;
