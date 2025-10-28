using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Banco.Queries.GetBancos;

public class GetBancosHandler : IRequestHandler<GetBancosQuery, List<BancoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBancosHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<BancoDto>> Handle(GetBancosQuery request, CancellationToken cancellationToken)
    {
        var bancos = await _unitOfWork.Repository<Domain.Entities.Banco>()
            .GetAllAsync(cancellationToken);

        // Aplicar filtro de estado si se proporciona
        if (!string.IsNullOrEmpty(request.Estado))
        {
            bancos = bancos.Where(b => b.Estado == request.Estado).ToList();
        }

        return bancos.Select(b => _mapper.Map<BancoDto>(b)).ToList();
    }
}
