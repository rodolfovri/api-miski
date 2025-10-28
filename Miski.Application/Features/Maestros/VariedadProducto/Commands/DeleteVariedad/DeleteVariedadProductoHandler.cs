using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.VariedadProducto.Commands.DeleteVariedad;

public class DeleteVariedadProductoHandler : IRequestHandler<DeleteVariedadProductoCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVariedadProductoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteVariedadProductoCommand request, CancellationToken cancellationToken)
    {
        var variedad = await _unitOfWork.Repository<Domain.Entities.VariedadProducto>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (variedad == null)
            throw new NotFoundException("VariedadProducto", request.Id);

        // Cambiar estado a INACTIVO en lugar de eliminar físicamente
        variedad.Estado = "INACTIVO";
        await _unitOfWork.Repository<Domain.Entities.VariedadProducto>().UpdateAsync(variedad, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
