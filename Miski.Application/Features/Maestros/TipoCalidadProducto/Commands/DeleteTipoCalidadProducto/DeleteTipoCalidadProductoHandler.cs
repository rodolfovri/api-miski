using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Commands.DeleteTipoCalidadProducto;

public class DeleteTipoCalidadProductoHandler : IRequestHandler<DeleteTipoCalidadProductoCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTipoCalidadProductoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteTipoCalidadProductoCommand request, CancellationToken cancellationToken)
    {
        var tipoCalidadProducto = await _unitOfWork.Repository<Domain.Entities.TipoCalidadProducto>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (tipoCalidadProducto == null)
            throw new NotFoundException("TipoCalidadProducto", request.Id);

        // Cambiar estado a INACTIVO en lugar de eliminar físicamente
        tipoCalidadProducto.Estado = "INACTIVO";
        
        await _unitOfWork.Repository<Domain.Entities.TipoCalidadProducto>()
            .UpdateAsync(tipoCalidadProducto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
