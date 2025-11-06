using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.VariedadProducto.Queries.GetVariedades;

public class GetVariedadesProductoHandler : IRequestHandler<GetVariedadesProductoQuery, List<VariedadProductoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVariedadesProductoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<VariedadProductoDto>> Handle(GetVariedadesProductoQuery request, CancellationToken cancellationToken)
    {
        var variedades = await _unitOfWork.Repository<Domain.Entities.VariedadProducto>().GetAllAsync(cancellationToken);
        var productos = await _unitOfWork.Repository<Producto>().GetAllAsync(cancellationToken);
        var unidades = await _unitOfWork.Repository<Domain.Entities.UnidadMedida>().GetAllAsync(cancellationToken);
        
        // Cargar todos los stocks si se proporciona IdUbicacion
        List<Stock> todosLosStocks = new List<Stock>();
        if (request.IdUbicacion.HasValue)
        {
            var stocks = await _unitOfWork.Repository<Stock>().GetAllAsync(cancellationToken);
            todosLosStocks = stocks.Where(s => s.IdPlanta == request.IdUbicacion.Value).ToList();
        }

        // Aplicar filtros
        if (!string.IsNullOrEmpty(request.Nombre))
        {
            variedades = variedades.Where(v =>
                v.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrEmpty(request.Codigo))
        {
            variedades = variedades.Where(v =>
                v.Codigo.Contains(request.Codigo, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (request.IdProducto.HasValue)
        {
            variedades = variedades.Where(v => v.IdProducto == request.IdProducto.Value).ToList();
        }

        if (!string.IsNullOrEmpty(request.Estado))
        {
            variedades = variedades.Where(v => v.Estado == request.Estado).ToList();
        }

        // Construir los DTOs con stock
        var resultado = new List<VariedadProductoDto>();

        foreach (var variedad in variedades)
        {
            // Cargar relaciones
            variedad.Producto = productos.FirstOrDefault(p => p.IdProducto == variedad.IdProducto) ?? new Producto();
            variedad.UnidadMedida = unidades.FirstOrDefault(u => u.IdUnidadMedida == variedad.IdUnidadMedida) ?? new Domain.Entities.UnidadMedida();

            // Mapear a DTO
            var dto = _mapper.Map<VariedadProductoDto>(variedad);

            // Buscar el stock para esta variedad en la ubicación especificada
            if (request.IdUbicacion.HasValue)
            {
                var stock = todosLosStocks.FirstOrDefault(s => s.IdVariedadProducto == variedad.IdVariedadProducto);
                dto.StockKg = stock?.CantidadKg ?? 0;  // Si no hay stock, retorna 0
            }
            else
            {
                dto.StockKg = 0;  // Si no se proporciona ubicación, stock es 0
            }

            resultado.Add(dto);
        }

        return resultado;
    }
}
