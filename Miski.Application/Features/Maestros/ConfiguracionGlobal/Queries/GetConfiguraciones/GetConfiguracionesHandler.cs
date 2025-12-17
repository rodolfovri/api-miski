using AutoMapper;
using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Queries.GetConfiguraciones;

public class GetConfiguracionesHandler : IRequestHandler<GetConfiguracionesQuery, List<ConfiguracionGlobalDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetConfiguracionesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ConfiguracionGlobalDto>> Handle(GetConfiguracionesQuery request, CancellationToken cancellationToken)
    {
        var configuraciones = await _unitOfWork.Repository<Domain.Entities.ConfiguracionGlobal>()
            .GetAllAsync(cancellationToken);

        return _mapper.Map<List<ConfiguracionGlobalDto>>(configuraciones.OrderBy(c => c.Clave).ToList());
    }
}
