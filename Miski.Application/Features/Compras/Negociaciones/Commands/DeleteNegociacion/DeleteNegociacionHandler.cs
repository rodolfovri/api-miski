using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.DeleteNegociacion;

public class DeleteNegociacionHandler : IRequestHandler<DeleteNegociacionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNegociacionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteNegociacionCommand request, CancellationToken cancellationToken)
    {
        var negociacion = await _unitOfWork.Repository<Negociacion>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (negociacion == null)
            throw new NotFoundException("Negociacion", request.Id);

        // Validar que no tenga compras asociadas
        var compras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);
        if (compras.Any(c => c.IdNegociacion == request.Id))
        {
            throw new ValidationException("No se puede eliminar la negociación porque tiene compras asociadas");
        }

        // Validar que no esté aprobada
        if (negociacion.EstadoAprobacionIngeniero == "APROBADO" || negociacion.Estado == "APROBADO")
        {
            throw new ValidationException("No se puede eliminar una negociación que ya ha sido aprobada");
        }

        // TODO: Eliminar fotos del almacenamiento si las hubiera

        // Cambiar estado a ANULADO
        negociacion.Estado = "ANULADO";
        await _unitOfWork.Repository<Negociacion>().UpdateAsync(negociacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
