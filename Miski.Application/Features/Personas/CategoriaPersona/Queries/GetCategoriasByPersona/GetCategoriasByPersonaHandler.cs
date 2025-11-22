using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Queries.GetCategoriasByPersona;

public class GetCategoriasByPersonaHandler : IRequestHandler<GetCategoriasByPersonaQuery, List<PersonaCategoriaDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoriasByPersonaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PersonaCategoriaDto>> Handle(GetCategoriasByPersonaQuery request, CancellationToken cancellationToken)
    {
        // Obtener todas las relaciones PersonaCategoria
        var personaCategorias = await _unitOfWork.Repository<PersonaCategoria>()
            .GetAllAsync(cancellationToken);

        // Filtrar por persona
        var categoriasDePersona = personaCategorias
            .Where(pc => pc.IdPersona == request.IdPersona)
            .ToList();

        // Obtener todas las categorías para mapear
        var categorias = await _unitOfWork.Repository<Domain.Entities.CategoriaPersona>()
            .GetAllAsync(cancellationToken);

        // Crear los DTOs con información de la categoría
        var resultado = categoriasDePersona.Select(pc =>
        {
            var categoria = categorias.FirstOrDefault(c => c.IdCategoriaPersona == pc.IdCategoria);

            return new PersonaCategoriaDto
            {
                IdPersonaCategoria = pc.IdPersonaCategoria,
                IdPersona = pc.IdPersona,
                IdCategoria = pc.IdCategoria,
                CategoriaNombre = categoria?.Nombre
            };
        }).ToList();

        return resultado;
    }
}
