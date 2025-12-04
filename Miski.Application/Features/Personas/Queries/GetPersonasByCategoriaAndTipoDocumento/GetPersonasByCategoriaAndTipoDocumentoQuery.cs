using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.Queries.GetPersonasByCategoriaAndTipoDocumento;

public record GetPersonasByCategoriaAndTipoDocumentoQuery(
    int IdCategoriaPersona,
    int IdTipoDocumento
) : IRequest<List<PersonaDto>>;
