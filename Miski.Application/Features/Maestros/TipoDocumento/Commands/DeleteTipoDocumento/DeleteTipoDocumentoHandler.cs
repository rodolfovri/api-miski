using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoDocumento.Commands.DeleteTipoDocumento;

public class DeleteTipoDocumentoHandler : IRequestHandler<DeleteTipoDocumentoCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTipoDocumentoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTipoDocumentoCommand request, CancellationToken cancellationToken)
    {
        var tipoDocumento = await _unitOfWork.Repository<Domain.Entities.TipoDocumento>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (tipoDocumento == null)
            throw new NotFoundException("TipoDocumento", request.Id);

        // Verificar si hay personas usando este tipo de documento
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var tienePersonas = personas.Any(p => p.IdTipoDocumento == request.Id);

        if (tienePersonas)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "TipoDocumento", new[] { "No se puede eliminar el tipo de documento porque est� siendo utilizado por personas" } }
            });
        }

        await _unitOfWork.Repository<Domain.Entities.TipoDocumento>().DeleteAsync(tipoDocumento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}