using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Queries.GetTipoCalidadProductos;

public class GetTipoCalidadProductosHandler : IRequestHandler<GetTipoCalidadProductosQuery, List<TipoCalidadProductoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTipoCalidadProductosHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<TipoCalidadProductoDto>> Handle(GetTipoCalidadProductosQuery request, CancellationToken cancellationToken)
    {
        var tipoCalidadProductos = await _unitOfWork.Repository<Domain.Entities.TipoCalidadProducto>()
            .GetAllAsync(cancellationToken);
        
        var productos = await _unitOfWork.Repository<Producto>().GetAllAsync(cancellationToken);

        // Aplicar filtros
        if (request.IdProducto.HasValue)
        {
            tipoCalidadProductos = tipoCalidadProductos
                .Where(t => t.IdProducto == request.IdProducto.Value)
                .ToList();
        }

        if (!string.IsNullOrEmpty(request.Estado))
        {
            tipoCalidadProductos = tipoCalidadProductos
                .Where(t => t.Estado == request.Estado)
                .ToList();
        }

        // Cargar relaciones
        foreach (var tipoCalidad in tipoCalidadProductos)
        {
            tipoCalidad.Producto = productos.FirstOrDefault(p => p.IdProducto == tipoCalidad.IdProducto);
        }

        return tipoCalidadProductos
            .Select(t => _mapper.Map<TipoCalidadProductoDto>(t))
            .ToList();
    }
}
