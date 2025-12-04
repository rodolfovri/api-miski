using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.Queries.GetPersonasByCategoriaAndTipoDocumento;

public class GetPersonasByCategoriaAndTipoDocumentoHandler : IRequestHandler<GetPersonasByCategoriaAndTipoDocumentoQuery, List<PersonaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPersonasByCategoriaAndTipoDocumentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PersonaDto>> Handle(GetPersonasByCategoriaAndTipoDocumentoQuery request, CancellationToken cancellationToken)
    {
        // Validar que la categoría existe
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaPersona>()
            .GetByIdAsync(request.IdCategoriaPersona, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaPersona", request.IdCategoriaPersona);

        // Validar que el tipo de documento existe
        var tipoDocumento = await _unitOfWork.Repository<TipoDocumento>()
            .GetByIdAsync(request.IdTipoDocumento, cancellationToken);

        if (tipoDocumento == null)
            throw new NotFoundException("TipoDocumento", request.IdTipoDocumento);

        // Obtener todas las personas con la categoría especificada
        var personaCategorias = await _unitOfWork.Repository<PersonaCategoria>()
            .GetAllAsync(cancellationToken);

        var personaIds = personaCategorias
            .Where(pc => pc.IdCategoria == request.IdCategoriaPersona)
            .Select(pc => pc.IdPersona)
            .ToList();

        // Obtener personas que coincidan con el tipo de documento
        var personas = await _unitOfWork.Repository<Persona>()
            .GetAllAsync(cancellationToken);

        var personasFiltradas = personas
            .Where(p => personaIds.Contains(p.IdPersona) && 
                       p.IdTipoDocumento == request.IdTipoDocumento)
            .ToList();

        // Cargar relaciones
        foreach (var persona in personasFiltradas)
        {
            persona.TipoDocumento = tipoDocumento;
        }

        return personasFiltradas.Select(p => _mapper.Map<PersonaDto>(p)).ToList();
    }
}
