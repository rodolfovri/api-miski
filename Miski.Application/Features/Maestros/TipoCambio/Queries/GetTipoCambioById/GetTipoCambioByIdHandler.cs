using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoCambio.Queries.GetTipoCambioById;

public class GetTipoCambioByIdHandler : IRequestHandler<GetTipoCambioByIdQuery, TipoCambioDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTipoCambioByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TipoCambioDto> Handle(GetTipoCambioByIdQuery request, CancellationToken cancellationToken)
    {
        var tipoCambio = await _unitOfWork.Repository<Domain.Entities.TipoCambio>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (tipoCambio == null)
            throw new NotFoundException("TipoCambio", request.Id);

        // Cargar relaciones
        tipoCambio.Moneda = await _unitOfWork.Repository<Domain.Entities.Moneda>()
            .GetByIdAsync(tipoCambio.IdMoneda, cancellationToken);

        tipoCambio.Usuario = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(tipoCambio.IdUsuario, cancellationToken);

        return _mapper.Map<TipoCambioDto>(tipoCambio);
    }
}
