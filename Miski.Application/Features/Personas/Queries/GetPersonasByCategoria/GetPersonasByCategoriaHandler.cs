using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;
using Miski.Shared.Exceptions;
using CategoriaPersonaEntity = Miski.Domain.Entities.CategoriaPersona;

namespace Miski.Application.Features.Personas.Queries.GetPersonasByCategoria;

public class GetPersonasByCategoriaHandler : IRequestHandler<GetPersonasByCategoriaQuery, List<PersonaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPersonasByCategoriaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PersonaDto>> Handle(GetPersonasByCategoriaQuery request, CancellationToken cancellationToken)
    {
        // Verificar que la categoría existe
        var categoria = await _unitOfWork.Repository<CategoriaPersonaEntity>()
            .GetByIdAsync(request.IdCategoria, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaPersona", request.IdCategoria);

        // Obtener todas las relaciones PersonaCategoria
        var personaCategorias = await _unitOfWork.Repository<PersonaCategoria>()
            .GetAllAsync(cancellationToken);

        // Filtrar por la categoría solicitada
        var personaIdsEnCategoria = personaCategorias
            .Where(pc => pc.IdCategoria == request.IdCategoria)
            .Select(pc => pc.IdPersona)
            .ToList();

        // Obtener todas las personas
        var todasPersonas = await _unitOfWork.Repository<Persona>()
            .GetAllAsync(cancellationToken);

        // Filtrar personas que pertenecen a la categoría
        var personas = todasPersonas
            .Where(p => personaIdsEnCategoria.Contains(p.IdPersona))
            .ToList();

        // Aplicar filtro por estado si se proporciona
        if (!string.IsNullOrEmpty(request.Estado))
        {
            personas = personas
                .Where(p => p.Estado == request.Estado)
                .ToList();
        }

        // Cargar relaciones - TipoDocumento
        var tiposDocumento = await _unitOfWork.Repository<TipoDocumento>()
            .GetAllAsync(cancellationToken);

        foreach (var persona in personas)
        {
            persona.TipoDocumento = tiposDocumento.FirstOrDefault(td => 
                td.IdTipoDocumento == persona.IdTipoDocumento);
        }

        return personas.Select(p => _mapper.Map<PersonaDto>(p)).ToList();
    }
}
