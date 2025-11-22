using MediatR;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Application.Features.Ubicaciones.Queries.GetUbicacionesByPersona;

public record GetUbicacionesByPersonaQuery(int IdPersona) : IRequest<List<PersonaUbicacionDto>>;
