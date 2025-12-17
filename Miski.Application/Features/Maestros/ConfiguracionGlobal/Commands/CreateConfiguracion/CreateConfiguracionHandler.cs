using AutoMapper;
using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.CreateConfiguracion;

public class CreateConfiguracionHandler : IRequestHandler<CreateConfiguracionCommand, ConfiguracionGlobalDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateConfiguracionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ConfiguracionGlobalDto> Handle(CreateConfiguracionCommand request, CancellationToken cancellationToken)
    {
        // Verificar que no exista una configuración con la misma clave
        var configuraciones = await _unitOfWork.Repository<Domain.Entities.ConfiguracionGlobal>()
            .GetAllAsync(cancellationToken);

        if (configuraciones.Any(c => c.Clave.Equals(request.ConfiguracionData.Clave, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ValidationException($"Ya existe una configuración con la clave '{request.ConfiguracionData.Clave}'");
        }

        var configuracion = new Domain.Entities.ConfiguracionGlobal
        {
            Clave = request.ConfiguracionData.Clave,
            Valor = request.ConfiguracionData.Valor,
            Descripcion = request.ConfiguracionData.Descripcion,
            TipoDato = request.ConfiguracionData.TipoDato.ToLower(),
            EsEditable = request.ConfiguracionData.EsEditable,
            FRegistro = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Domain.Entities.ConfiguracionGlobal>().AddAsync(configuracion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ConfiguracionGlobalDto>(configuracion);
    }
}
