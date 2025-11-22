using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Cargo.Queries.GetCargos;

public class GetCargosHandler : IRequestHandler<GetCargosQuery, List<CargoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCargosHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CargoDto>> Handle(GetCargosQuery request, CancellationToken cancellationToken)
    {
        var cargos = await _unitOfWork.Repository<Domain.Entities.Cargo>()
            .GetAllAsync(cancellationToken);

        // Aplicar filtro de estado si se proporciona
        if (!string.IsNullOrEmpty(request.Estado))
        {
            cargos = cargos.Where(c => c.Estado == request.Estado).ToList();
        }

        return cargos.Select(c => _mapper.Map<CargoDto>(c)).ToList();
    }
}
