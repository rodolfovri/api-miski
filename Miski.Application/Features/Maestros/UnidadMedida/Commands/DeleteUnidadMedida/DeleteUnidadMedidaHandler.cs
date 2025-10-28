using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.UnidadMedida.Commands.DeleteUnidadMedida;

public class DeleteUnidadMedidaHandler : IRequestHandler<DeleteUnidadMedidaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUnidadMedidaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteUnidadMedidaCommand request, CancellationToken cancellationToken)
    {
        var unidadMedida = await _unitOfWork.Repository<Domain.Entities.UnidadMedida>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (unidadMedida == null)
            throw new NotFoundException("UnidadMedida", request.Id);

        // Verificar si hay variedades de productos usando esta unidad de medida
        var variedades = await _unitOfWork.Repository<Domain.Entities.VariedadProducto>().GetAllAsync(cancellationToken);
        var tieneVariedades = variedades.Any(v => v.IdUnidadMedida == request.Id);

        if (tieneVariedades)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "UnidadMedida", new[] { "No se puede eliminar la unidad de medida porque está siendo utilizada por variedades de productos" } }
            });
        }

        await _unitOfWork.Repository<Domain.Entities.UnidadMedida>().DeleteAsync(unidadMedida, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}