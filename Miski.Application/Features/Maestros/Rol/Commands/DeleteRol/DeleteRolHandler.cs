using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Rol.Commands.DeleteRol;

public class DeleteRolHandler : IRequestHandler<DeleteRolCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRolHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteRolCommand request, CancellationToken cancellationToken)
    {
        var rol = await _unitOfWork.Repository<Domain.Entities.Rol>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (rol == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Rol), request.Id);
        }

        // Soft delete: cambiar estado a INACTIVO en lugar de eliminar físicamente
        rol.Estado = "INACTIVO";

        await _unitOfWork.Repository<Domain.Entities.Rol>()
            .UpdateAsync(rol, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
