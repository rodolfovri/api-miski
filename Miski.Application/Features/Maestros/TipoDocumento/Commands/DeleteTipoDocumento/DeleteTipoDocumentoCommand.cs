using MediatR;

namespace Miski.Application.Features.Maestros.TipoDocumento.Commands.DeleteTipoDocumento;

public record DeleteTipoDocumentoCommand(int Id) : IRequest<bool>;