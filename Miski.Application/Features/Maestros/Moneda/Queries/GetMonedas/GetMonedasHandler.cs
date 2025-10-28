using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Moneda.Queries.GetMonedas;

public class GetMonedasHandler : IRequestHandler<GetMonedasQuery, IEnumerable<MonedaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMonedasHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MonedaDto>> Handle(GetMonedasQuery request, CancellationToken cancellationToken)
    {
        var monedas = await _unitOfWork.Repository<Domain.Entities.Moneda>()
            .GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<MonedaDto>>(monedas);
    }
}
