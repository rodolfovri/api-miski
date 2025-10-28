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

        // Cargar relaciones
        foreach (var variedad in variedades)
        {
            variedad.Producto = productos.FirstOrDefault(p => p.IdProducto == variedad.IdProducto) ?? new Producto();
            variedad.UnidadMedida = unidades.FirstOrDefault(u => u.IdUnidadMedida == variedad.IdUnidadMedida) ?? new Domain.Entities.UnidadMedida();
        }

        return variedades.Select(v => _mapper.Map<VariedadProductoDto>(v)).ToList();
    }
}
