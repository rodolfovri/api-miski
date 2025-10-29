using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Lotes.Commands.DeleteLote;

public class DeleteLoteHandler : IRequestHandler<DeleteLoteCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLoteHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteLoteCommand request, CancellationToken cancellationToken)
    {
        var lote = await _unitOfWork.Repository<Lote>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (lote == null)
            throw new NotFoundException("Lote", request.Id);

        // Validar que el lote no tenga llegadas de planta asociadas
        var llegadasPlanta = await _unitOfWork.Repository<LlegadaPlanta>().GetAllAsync(cancellationToken);
        if (llegadasPlanta.Any(lp => lp.IdLote == request.Id))
        {
            throw new ValidationException("No se puede eliminar el lote porque tiene llegadas de planta asociadas");
        }

        // Eliminar físicamente el lote
        await _unitOfWork.Repository<Lote>().DeleteAsync(lote, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
