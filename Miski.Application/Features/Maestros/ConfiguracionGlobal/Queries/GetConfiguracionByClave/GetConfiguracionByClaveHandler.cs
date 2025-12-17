using AutoMapper;
using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Queries.GetConfiguracionByClave;

public class GetConfiguracionByClaveHandler : IRequestHandler<GetConfiguracionByClaveQuery, ConfiguracionGlobalDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetConfiguracionByClaveHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ConfiguracionGlobalDto> Handle(GetConfiguracionByClaveQuery request, CancellationToken cancellationToken)
    {
        var configuraciones = await _unitOfWork.Repository<Domain.Entities.ConfiguracionGlobal>()
            .GetAllAsync(cancellationToken);

        var configuracion = configuraciones.FirstOrDefault(c => 
            c.Clave.Equals(request.Clave, StringComparison.OrdinalIgnoreCase));

        if (configuracion == null)
        {
            throw new NotFoundException("Configuración", request.Clave);
        }

        return _mapper.Map<ConfiguracionGlobalDto>(configuracion);
    }
}
