using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Queries.GetTipoCalidadByProductoId;

public class GetTipoCalidadByProductoIdHandler : IRequestHandler<GetTipoCalidadByProductoIdQuery, List<TipoCalidadProductoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTipoCalidadByProductoIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<TipoCalidadProductoDto>> Handle(GetTipoCalidadByProductoIdQuery request, CancellationToken cancellationToken)
    {
        // Verificar que el producto exista
        var producto = await _unitOfWork.Repository<Producto>()
            .GetByIdAsync(request.IdProducto, cancellationToken);

        if (producto == null)
        {
            throw new NotFoundException(nameof(Producto), request.IdProducto);
        }

        // Obtener todos los tipos de calidad del producto
        var tipoCalidadProductos = await _unitOfWork.Repository<Domain.Entities.TipoCalidadProducto>()
            .GetAllAsync(cancellationToken);

        // Filtrar por el producto especificado
        var tiposCalidadDelProducto = tipoCalidadProductos
            .Where(t => t.IdProducto == request.IdProducto)
            .ToList();

        // Cargar la relación con el producto
        foreach (var tipoCalidad in tiposCalidadDelProducto)
        {
            tipoCalidad.Producto = producto;
        }

        return tiposCalidadDelProducto
            .Select(t => _mapper.Map<TipoCalidadProductoDto>(t))
            .ToList();
    }
}
