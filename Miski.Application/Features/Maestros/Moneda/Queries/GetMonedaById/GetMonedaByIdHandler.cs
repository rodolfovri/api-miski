using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Moneda.Queries.GetMonedaById;

public class GetMonedaByIdHandler : IRequestHandler<GetMonedaByIdQuery, MonedaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMonedaByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MonedaDto> Handle(GetMonedaByIdQuery request, CancellationToken cancellationToken)
    {
        var moneda = await _unitOfWork.Repository<Domain.Entities.Moneda>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (moneda == null)
            throw new NotFoundException("Moneda", request.Id);

        return _mapper.Map<MonedaDto>(moneda);
    }
}
