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

        // Validar que el motivo de anulaci�n no est� vac�o
        if (string.IsNullOrWhiteSpace(request.MotivoAnulacion))
        {
            throw new ValidationException("El motivo de anulaci�n es obligatorio");
        }

        // Validar los estados permitidos para anulaci�n
        bool puedeAnular = false;

        // Caso 1: Estado EN PROCESO y EstadoAprobacionIngeniero PENDIENTE
        if (negociacion.Estado == "EN PROCESO" && negociacion.EstadoAprobacionIngeniero == "PENDIENTE")
        {
            puedeAnular = true;
        }
        // Caso 2: Estado APROBADO y EstadoAprobacionIngeniero APROBADO
        else if (negociacion.Estado == "APROBADO" && negociacion.EstadoAprobacionIngeniero == "APROBADO")
        {
            puedeAnular = true;
        }
        // Caso 3: Estado EN REVISI�N y EstadoAprobacionContadora PENDIENTE
        else if (negociacion.Estado == "EN REVISI�N" && negociacion.EstadoAprobacionContadora == "PENDIENTE")
        {
            puedeAnular = true;
        }

        if (!puedeAnular)
        {
            throw new ValidationException("No se puede anular la negociaci�n en su estado actual. Solo se pueden anular negociaciones en estados: EN PROCESO (Pendiente de ingeniero), APROBADO (Aprobado por ingeniero) o EN REVISI�N (Pendiente de contadora)");
        }

        // Anular la negociaci�n
        negociacion.Estado = "ANULADO";
        negociacion.EstadoAprobacionIngeniero = "ANULADO";
        negociacion.EstadoAprobacionContadora = "ANULADO";
        negociacion.IdUsuarioAnulacion = request.IdUsuarioAnulacion;
        negociacion.MotivoAnulacion = request.MotivoAnulacion;
        negociacion.FAnulacion = DateTime.Now;

        await _unitOfWork.Repository<Negociacion>().UpdateAsync(negociacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
