using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoDocumento.Queries.GetTipoDocumento;

public record GetTipoDocumentoQuery(
    string? Nombre = null
) : IRequest<List<TipoDocumentoDto>>;
