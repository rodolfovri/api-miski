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

        // ✅ Cargar relación inversa con Compra (relación 1:1)
        var compras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);
        var compraAsociada = compras.FirstOrDefault(c => c.IdLote == lote.IdLote);
        
        if (compraAsociada != null)
        {
            lote.Compra = compraAsociada;
        }

        var loteDto = _mapper.Map<LoteDto>(lote);
        
        // ✅ Agregar MontoTotal de la compra asociada
        if (compraAsociada != null)
        {
            loteDto.IdCompra = compraAsociada.IdCompra;
            loteDto.MontoTotal = compraAsociada.MontoTotal;
        }

        return loteDto;
    }
}
