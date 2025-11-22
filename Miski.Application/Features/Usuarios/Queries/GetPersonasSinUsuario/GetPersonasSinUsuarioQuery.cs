using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Usuarios.Queries.GetPersonasSinUsuario;

public record GetPersonasSinUsuarioQuery(string? Estado = null, int? IdUsuario = null) : IRequest<List<PersonaDto>>;
