using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.DeleteConfiguracion;

public class DeleteConfiguracionHandler : IRequestHandler<DeleteConfiguracionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteConfiguracionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteConfiguracionCommand request, CancellationToken cancellationToken)
    {
        var configuracion = await _unitOfWork.Repository<Domain.Entities.ConfiguracionGlobal>()
            .GetByIdAsync(request.IdConfiguracionGlobal, cancellationToken);

        if (configuracion == null)
        {
            throw new NotFoundException("Configuración", request.IdConfiguracionGlobal);
        }

        if (!configuracion.EsEditable)
        {
            throw new ValidationException("Esta configuración no puede ser eliminada");
        }

        await _unitOfWork.Repository<Domain.Entities.ConfiguracionGlobal>().DeleteAsync(configuracion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
