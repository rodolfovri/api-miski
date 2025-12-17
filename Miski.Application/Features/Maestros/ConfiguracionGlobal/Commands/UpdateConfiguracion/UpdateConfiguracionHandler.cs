using AutoMapper;
using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.UpdateConfiguracion;

public class UpdateConfiguracionHandler : IRequestHandler<UpdateConfiguracionCommand, ConfiguracionGlobalDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateConfiguracionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ConfiguracionGlobalDto> Handle(UpdateConfiguracionCommand request, CancellationToken cancellationToken)
    {
        var configuracion = await _unitOfWork.Repository<Domain.Entities.ConfiguracionGlobal>()
            .GetByIdAsync(request.ConfiguracionData.IdConfiguracionGlobal, cancellationToken);

        if (configuracion == null)
        {
            throw new NotFoundException("Configuración", request.ConfiguracionData.IdConfiguracionGlobal);
        }

        if (!configuracion.EsEditable)
        {
            throw new ValidationException("Esta configuración no es editable");
        }

        configuracion.Valor = request.ConfiguracionData.Valor;
        configuracion.Descripcion = request.ConfiguracionData.Descripcion;
        configuracion.FModificacion = DateTime.UtcNow;

        await _unitOfWork.Repository<Domain.Entities.ConfiguracionGlobal>().UpdateAsync(configuracion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ConfiguracionGlobalDto>(configuracion);
    }
}
