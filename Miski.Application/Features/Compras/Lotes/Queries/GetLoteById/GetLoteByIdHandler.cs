using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Lotes.Queries.GetLoteById;

public class GetLoteByIdHandler : IRequestHandler<GetLoteByIdQuery, LoteDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLoteByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoteDto> Handle(GetLoteByIdQuery request, CancellationToken cancellationToken)
    {
        var lote = await _unitOfWork.Repository<Lote>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (lote == null)
            throw new NotFoundException("Lote", request.Id);

        // Cargar relación con Compra
        lote.Compra = await _unitOfWork.Repository<Compra>()
            .GetByIdAsync(lote.IdCompra, cancellationToken);

        return _mapper.Map<LoteDto>(lote);
    }
}
