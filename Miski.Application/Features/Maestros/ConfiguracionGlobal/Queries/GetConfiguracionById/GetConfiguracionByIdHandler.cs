using AutoMapper;
using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Queries.GetConfiguracionById;

public class GetConfiguracionByIdHandler : IRequestHandler<GetConfiguracionByIdQuery, ConfiguracionGlobalDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetConfiguracionByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ConfiguracionGlobalDto> Handle(GetConfiguracionByIdQuery request, CancellationToken cancellationToken)
    {
        var configuracion = await _unitOfWork.Repository<Domain.Entities.ConfiguracionGlobal>()
            .GetByIdAsync(request.IdConfiguracionGlobal, cancellationToken);

        if (configuracion == null)
        {
            throw new NotFoundException("Configuración", request.IdConfiguracionGlobal);
        }

        return _mapper.Map<ConfiguracionGlobalDto>(configuracion);
    }
}
