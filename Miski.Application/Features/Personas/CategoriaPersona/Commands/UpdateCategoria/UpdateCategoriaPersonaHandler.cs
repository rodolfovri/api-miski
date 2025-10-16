using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.UpdateCategoria;

public class UpdateCategoriaPersonaHandler : IRequestHandler<UpdateCategoriaPersonaCommand, CategoriaPersonaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoriaPersonaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoriaPersonaDto> Handle(UpdateCategoriaPersonaCommand request, CancellationToken cancellationToken)
    {
        // Buscar la categor�a
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaPersona>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaPersona", request.Id);

        // Verificar que no exista otra categor�a con el mismo nombre
        var categoriasExistentes = await _unitOfWork.Repository<Domain.Entities.CategoriaPersona>().GetAllAsync(cancellationToken);
        var existe = categoriasExistentes.Any(c =>
            c.Nombre.ToLower() == request.Categoria.Nombre.ToLower() &&
            c.IdCategoriaPersona != request.Id);

        if (existe)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "Nombre", new[] { "Ya existe otra categor�a con este nombre" } }
            });
        }

        // Actualizar la categor�a
        categoria.Nombre = request.Categoria.Nombre;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoriaPersonaDto>(categoria);
    }
}