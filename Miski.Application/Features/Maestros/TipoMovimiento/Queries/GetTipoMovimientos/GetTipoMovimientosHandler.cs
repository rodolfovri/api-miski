using AutoMapper;
using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Queries.GetTipoMovimientos;

public class GetTipoMovimientosHandler : IRequestHandler<GetTipoMovimientosQuery, List<TipoMovimientoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTipoMovimientosHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<TipoMovimientoDto>> Handle(GetTipoMovimientosQuery request, CancellationToken cancellationToken)
    {
        var tipoMovimientos = await _unitOfWork.Repository<Domain.Entities.TipoMovimiento>()
            .GetAllAsync(cancellationToken);

        return _mapper.Map<List<TipoMovimientoDto>>(tipoMovimientos.OrderBy(t => t.TipoOperacion).ToList());
    }
}
