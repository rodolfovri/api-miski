using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Almacen;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Almacen.Productos.Commands.UpdateProducto;

public class UpdateProductoHandler : IRequestHandler<UpdateProductoCommand, ProductoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductoDto> Handle(UpdateProductoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Producto;

        var producto = await _unitOfWork.Repository<Producto>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (producto == null)
            throw new NotFoundException("Producto", request.Id);

        // Validar que la categoría existe
        var categoria = await _unitOfWork.Repository<CategoriaProducto>()
            .GetByIdAsync(dto.IdCategoriaProducto, cancellationToken);
        
        if (categoria == null)
            throw new NotFoundException("CategoriaProducto", dto.IdCategoriaProducto);

        // Validar que la unidad de medida existe
        var unidadMedida = await _unitOfWork.Repository<UnidadMedida>()
            .GetByIdAsync(dto.IdUnidadMedida, cancellationToken);
        
        if (unidadMedida == null)
            throw new NotFoundException("UnidadMedida", dto.IdUnidadMedida);

        // Validar que el código no esté duplicado (excepto el mismo producto)
        var productos = await _unitOfWork.Repository<Producto>().GetAllAsync(cancellationToken);
        if (productos.Any(p => p.Codigo == dto.Codigo && p.IdProducto != request.Id))
            throw new ValidationException($"Ya existe otro producto con el código {dto.Codigo}");

        // Actualizar producto
        producto.IdCategoriaProducto = dto.IdCategoriaProducto;
        producto.IdUnidadMedida = dto.IdUnidadMedida;
        producto.Codigo = dto.Codigo;
        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.Estado = dto.Estado ?? producto.Estado;

        await _unitOfWork.Repository<Producto>().UpdateAsync(producto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        producto.CategoriaProducto = categoria;
        producto.UnidadMedida = unidadMedida;

        return _mapper.Map<ProductoDto>(producto);
    }
}
