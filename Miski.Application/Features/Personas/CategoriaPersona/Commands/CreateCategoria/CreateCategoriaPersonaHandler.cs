using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.CreateCategoria;

public class CreateCategoriaPersonaHandler : IRequestHandler<CreateCategoriaPersonaCommand, CategoriaPersonaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoriaPersonaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoriaPersonaDto> Handle(CreateCategoriaPersonaCommand request, CancellationToken cancellationToken)
    {
        // Verificar que no exista una categor�a con el mismo nombre
        var categoriasExistentes = await _unitOfWork.Repository<Domain.Entities.CategoriaPersona>().GetAllAsync(cancellationToken);
        var existe = categoriasExistentes.Any(c => c.Nombre.ToLower() == request.Categoria.Nombre.ToLower());

        if (existe)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "Nombre", new[] { "Ya existe una categor�a con este nombre" } }
            });
        }

        // Crear la nueva categor�a
        var nuevaCategoria = new Domain.Entities.CategoriaPersona
        {
            Nombre = request.Categoria.Nombre
        };

        await _unitOfWork.Repository<Domain.Entities.CategoriaPersona>().AddAsync(nuevaCategoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoriaPersonaDto>(nuevaCategoria);
    }
}