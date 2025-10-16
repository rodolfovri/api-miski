using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Almacen;

namespace Miski.Application.Features.Almacen.Productos.Queries.GetProductos;

public class GetProductosHandler : IRequestHandler<GetProductosQuery, List<ProductoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductosHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ProductoDto>> Handle(GetProductosQuery request, CancellationToken cancellationToken)
    {
        var productos = await _unitOfWork.Repository<Producto>().GetAllAsync(cancellationToken);
        var categorias = await _unitOfWork.Repository<CategoriaProducto>().GetAllAsync(cancellationToken);
        var unidades = await _unitOfWork.Repository<UnidadMedida>().GetAllAsync(cancellationToken);

        // Aplicar filtros
        if (!string.IsNullOrEmpty(request.Nombre))
        {
            productos = productos.Where(p =>
                p.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrEmpty(request.Codigo))
        {
            productos = productos.Where(p =>
                p.Codigo.Contains(request.Codigo, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (request.IdCategoriaProducto.HasValue)
        {
            productos = productos.Where(p => p.IdCategoriaProducto == request.IdCategoriaProducto.Value).ToList();
        }

        if (!string.IsNullOrEmpty(request.Estado))
        {
            productos = productos.Where(p => p.Estado == request.Estado).ToList();
        }

        // Cargar relaciones
        foreach (var producto in productos)
        {
            producto.CategoriaProducto = categorias.FirstOrDefault(c => c.IdCategoriaProducto == producto.IdCategoriaProducto) ?? new CategoriaProducto();
            producto.UnidadMedida = unidades.FirstOrDefault(u => u.IdUnidadMedida == producto.IdUnidadMedida) ?? new UnidadMedida();
        }

        return productos.Select(p => _mapper.Map<ProductoDto>(p)).ToList();
    }
}
