using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Cargo.Commands.DeleteCargo;

public class DeleteCargoHandler : IRequestHandler<DeleteCargoCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCargoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCargoCommand request, CancellationToken cancellationToken)
    {
        var cargo = await _unitOfWork.Repository<Domain.Entities.Cargo>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (cargo == null)
        {
            throw new NotFoundException("Cargo", request.Id);
        }

        // Soft delete: cambiar estado a INACTIVO
        cargo.Estado = "INACTIVO";
        
        await _unitOfWork.Repository<Domain.Entities.Cargo>()
            .UpdateAsync(cargo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
