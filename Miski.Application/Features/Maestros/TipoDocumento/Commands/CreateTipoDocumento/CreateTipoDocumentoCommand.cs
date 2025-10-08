using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoDocumento.Commands.CreateTipoDocumento;

public record CreateTipoDocumentoCommand(CreateTipoDocumentoDto TipoDocumento) : IRequest<TipoDocumentoDto>;