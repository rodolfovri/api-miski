using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoDocumento.Queries.GetTipoDocumentoById;

public record GetTipoDocumentoByIdQuery(int Id) : IRequest<TipoDocumentoDto>;