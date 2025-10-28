using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Banco.Commands.DeleteBanco;

public class DeleteBancoHandler : IRequestHandler<DeleteBancoCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBancoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteBancoCommand request, CancellationToken cancellationToken)
    {
        var banco = await _unitOfWork.Repository<Domain.Entities.Banco>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (banco == null)
            throw new NotFoundException("Banco", request.Id);

        // Cambiar estado a INACTIVO en lugar de eliminar físicamente
        banco.Estado = "INACTIVO";
        
        await _unitOfWork.Repository<Domain.Entities.Banco>()
            .UpdateAsync(banco, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
