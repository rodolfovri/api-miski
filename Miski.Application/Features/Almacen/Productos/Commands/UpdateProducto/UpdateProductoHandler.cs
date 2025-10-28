using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Almacen;
using Miski.Shared.Exceptions;
using Miski.Application.Services;

namespace Miski.Application.Features.Almacen.Productos.Commands.UpdateProducto;

public class UpdateProductoHandler : IRequestHandler<UpdateProductoCommand, ProductoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;

    public UpdateProductoHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
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

        // Actualizar imagen si se proporciona una nueva
        if (dto.Imagen != null)
        {
            // Eliminar imagen anterior si existe
            if (!string.IsNullOrEmpty(producto.Imagen))
            {
                await _fileStorageService.DeleteFileAsync(producto.Imagen, cancellationToken);
            }

            producto.Imagen = await _fileStorageService.SaveFileAsync(
                dto.Imagen, 
                "productos/imagenes", 
                cancellationToken);
        }

        // Actualizar ficha técnica si se proporciona una nueva
        if (dto.FichaTecnica != null)
        {
            // Eliminar ficha técnica anterior si existe
            if (!string.IsNullOrEmpty(producto.FichaTecnica))
            {
                await _fileStorageService.DeleteFileAsync(producto.FichaTecnica, cancellationToken);
            }

            producto.FichaTecnica = await _fileStorageService.SaveFileAsync(
                dto.FichaTecnica, 
                "productos/fichas-tecnicas", 
                cancellationToken);
        }

        // Actualizar producto
        producto.IdCategoriaProducto = dto.IdCategoriaProducto;
        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.Estado = dto.Estado ?? producto.Estado;

        await _unitOfWork.Repository<Producto>().UpdateAsync(producto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        producto.CategoriaProducto = categoria;

        return _mapper.Map<ProductoDto>(producto);
    }
}
