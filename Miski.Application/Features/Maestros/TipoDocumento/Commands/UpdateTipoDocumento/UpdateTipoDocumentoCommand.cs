using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoDocumento.Commands.UpdateTipoDocumento;

public record UpdateTipoDocumentoCommand(int Id, UpdateTipoDocumentoDto TipoDocumento) : IRequest<TipoDocumentoDto>;