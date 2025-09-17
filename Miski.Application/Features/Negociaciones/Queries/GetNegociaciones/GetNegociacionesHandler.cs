using AutoMapper;
using MediatR;
using Miski.Domain.Contracts.Repositories;
using Miski.Domain.Entities;
using Miski.Shared.DTOs;

namespace Miski.Application.Features.Negociaciones.Queries.GetNegociaciones;

public class GetNegociacionesHandler : IRequestHandler<GetNegociacionesQuery, IEnumerable<NegociacionDto>>
{
    private readonly INegociacionRepository _repository;
    private readonly IMapper _mapper;

    public GetNegociacionesHandler(INegociacionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NegociacionDto>> Handle(GetNegociacionesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Negociacion> negociaciones;

        if (request.ProveedorId.HasValue)
        {
            negociaciones = await _repository.GetByProveedorIdAsync(request.ProveedorId.Value, cancellationToken);
        }
        else if (request.ComisionistaId.HasValue)
        {
            negociaciones = await _repository.GetByComisionistaIdAsync(request.ComisionistaId.Value, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(request.Estado))
        {
            negociaciones = await _repository.GetByEstadoAsync(request.Estado, cancellationToken);
        }
        else
        {
            negociaciones = await _repository.GetAllAsync(cancellationToken);
        }

        return _mapper.Map<IEnumerable<NegociacionDto>>(negociaciones);
    }
}