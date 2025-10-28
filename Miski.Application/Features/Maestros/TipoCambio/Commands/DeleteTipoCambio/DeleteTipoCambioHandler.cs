using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoCambio.Commands.DeleteTipoCambio;

public class DeleteTipoCambioHandler : IRequestHandler<DeleteTipoCambioCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTipoCambioHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteTipoCambioCommand request, CancellationToken cancellationToken)
    {
        var tipoCambio = await _unitOfWork.Repository<Domain.Entities.TipoCambio>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (tipoCambio == null)
            throw new NotFoundException("TipoCambio", request.Id);

        await _unitOfWork.Repository<Domain.Entities.TipoCambio>().DeleteAsync(tipoCambio, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
