using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Commands.DeleteTipoMovimiento;

public class DeleteTipoMovimientoHandler : IRequestHandler<DeleteTipoMovimientoCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTipoMovimientoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTipoMovimientoCommand request, CancellationToken cancellationToken)
    {
        var tipoMovimiento = await _unitOfWork.Repository<Domain.Entities.TipoMovimiento>()
            .GetByIdAsync(request.IdTipoMovimiento, cancellationToken);

        if (tipoMovimiento == null)
        {
            throw new NotFoundException("TipoMovimiento", request.IdTipoMovimiento);
        }

        tipoMovimiento.Estado = "INACTIVO";
        await _unitOfWork.Repository<Domain.Entities.TipoMovimiento>().UpdateAsync(tipoMovimiento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
