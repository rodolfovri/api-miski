using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Commands.CreateCategoria;

public class CreateCategoriaProductoHandler : IRequestHandler<CreateCategoriaProductoCommand, CategoriaProductoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoriaProductoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoriaProductoDto> Handle(CreateCategoriaProductoCommand request, CancellationToken cancellationToken)
    {
        // Verificar que no exista una categor�a con el mismo nombre
        var categoriasExistentes = await _unitOfWork.Repository<Domain.Entities.CategoriaProducto>().GetAllAsync(cancellationToken);
        var existe = categoriasExistentes.Any(c => c.Nombre.ToLower() == request.Categoria.Nombre.ToLower());

        if (existe)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "Nombre", new[] { "Ya existe una categor�a con este nombre" } }
            });
        }

        // Crear la nueva categor�a
        var nuevaCategoria = new Domain.Entities.CategoriaProducto
        {
            Nombre = request.Categoria.Nombre,
            Descripcion = request.Categoria.Descripcion,
            Estado = request.Categoria.Estado,
            FRegistro = DateTime.Now
        };

        await _unitOfWork.Repository<Domain.Entities.CategoriaProducto>().AddAsync(nuevaCategoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoriaProductoDto>(nuevaCategoria);
    }
}