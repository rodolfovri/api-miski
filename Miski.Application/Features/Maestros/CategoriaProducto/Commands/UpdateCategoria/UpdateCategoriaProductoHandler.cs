using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Commands.UpdateCategoria;

public class UpdateCategoriaProductoHandler : IRequestHandler<UpdateCategoriaProductoCommand, CategoriaProductoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoriaProductoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoriaProductoDto> Handle(UpdateCategoriaProductoCommand request, CancellationToken cancellationToken)
    {
        // Buscar la categor�a
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaProducto>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaProducto", request.Id);

        // Verificar que no exista otra categor�a con el mismo nombre
        var categoriasExistentes = await _unitOfWork.Repository<Domain.Entities.CategoriaProducto>().GetAllAsync(cancellationToken);
        var existe = categoriasExistentes.Any(c =>
            c.Nombre.ToLower() == request.Categoria.Nombre.ToLower() &&
            c.IdCategoriaProducto != request.Id);

        if (existe)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "Nombre", new[] { "Ya existe otra categor�a con este nombre" } }
            });
        }

        // Actualizar la categor�a
        categoria.Nombre = request.Categoria.Nombre;
        categoria.Descripcion = request.Categoria.Descripcion;
        
        if (!string.IsNullOrEmpty(request.Categoria.Estado))
        {
            categoria.Estado = request.Categoria.Estado;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoriaProductoDto>(categoria);
    }
}