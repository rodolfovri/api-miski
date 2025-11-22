using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.AsignarCategoria;

public class AsignarCategoriaHandler : IRequestHandler<AsignarCategoriaCommand, PersonaCategoriaDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public AsignarCategoriaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PersonaCategoriaDto> Handle(AsignarCategoriaCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

        // Validar que la persona existe
        var persona = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(dto.IdPersona, cancellationToken);

        if (persona == null)
        {
            throw new NotFoundException(nameof(Persona), dto.IdPersona);
        }

        // Validar que la categoría existe
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaPersona>()
            .GetByIdAsync(dto.IdCategoria, cancellationToken);

        if (categoria == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.CategoriaPersona), dto.IdCategoria);
        }

        // Verificar que no exista ya la asignación
        var personaCategorias = await _unitOfWork.Repository<PersonaCategoria>()
            .GetAllAsync(cancellationToken);

        var existeAsignacion = personaCategorias.Any(pc => 
            pc.IdPersona == dto.IdPersona && pc.IdCategoria == dto.IdCategoria);

        if (existeAsignacion)
        {
            throw new ValidationException("La persona ya tiene asignada esta categoría");
        }

        // Crear la nueva asignación
        var nuevaAsignacion = new PersonaCategoria
        {
            IdPersona = dto.IdPersona,
            IdCategoria = dto.IdCategoria
        };

        await _unitOfWork.Repository<PersonaCategoria>()
            .AddAsync(nuevaAsignacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el DTO
        return new PersonaCategoriaDto
        {
            IdPersonaCategoria = nuevaAsignacion.IdPersonaCategoria,
            IdPersona = nuevaAsignacion.IdPersona,
            IdCategoria = nuevaAsignacion.IdCategoria,
            CategoriaNombre = categoria.Nombre
        };
    }
}
