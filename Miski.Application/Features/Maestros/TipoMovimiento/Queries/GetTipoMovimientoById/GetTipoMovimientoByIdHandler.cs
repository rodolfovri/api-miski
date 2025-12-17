using AutoMapper;
using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Queries.GetTipoMovimientoById;

public class GetTipoMovimientoByIdHandler : IRequestHandler<GetTipoMovimientoByIdQuery, TipoMovimientoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTipoMovimientoByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TipoMovimientoDto> Handle(GetTipoMovimientoByIdQuery request, CancellationToken cancellationToken)
    {
        var tipoMovimiento = await _unitOfWork.Repository<Domain.Entities.TipoMovimiento>()
            .GetByIdAsync(request.IdTipoMovimiento, cancellationToken);

        if (tipoMovimiento == null)
        {
            throw new NotFoundException("TipoMovimiento", request.IdTipoMovimiento);
        }

        return _mapper.Map<TipoMovimientoDto>(tipoMovimiento);
    }
}
