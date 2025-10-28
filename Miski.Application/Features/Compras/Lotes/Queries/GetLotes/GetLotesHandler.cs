using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Lotes.Queries.GetLotes;

public class GetLotesHandler : IRequestHandler<GetLotesQuery, IEnumerable<LoteDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLotesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LoteDto>> Handle(GetLotesQuery request, CancellationToken cancellationToken)
    {
        var lotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);

        // Filtrar por compra si se especifica
        if (request.IdCompra.HasValue)
        {
            lotes = lotes.Where(l => l.IdCompra == request.IdCompra.Value);
        }

        // Filtrar por código si se especifica
        if (!string.IsNullOrEmpty(request.Codigo))
        {
            lotes = lotes.Where(l => l.Codigo != null && l.Codigo.Contains(request.Codigo, StringComparison.OrdinalIgnoreCase));
        }

        // Cargar relaciones
        var compras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);

        foreach (var lote in lotes)
        {
            lote.Compra = compras.FirstOrDefault(c => c.IdCompra == lote.IdCompra);
        }

        return lotes.Select(l => _mapper.Map<LoteDto>(l)).ToList();
    }
}
