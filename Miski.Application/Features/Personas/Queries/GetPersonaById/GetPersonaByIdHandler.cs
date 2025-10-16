using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.Queries.GetPersonaById;

public class GetPersonaByIdHandler : IRequestHandler<GetPersonaByIdQuery, PersonaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPersonaByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PersonaDto> Handle(GetPersonaByIdQuery request, CancellationToken cancellationToken)
    {
        var persona = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (persona == null)
            throw new NotFoundException("Persona", request.Id);

        // Cargar tipo de documento
        var tipoDocumento = await _unitOfWork.Repository<TipoDocumento>()
            .GetByIdAsync(persona.IdTipoDocumento, cancellationToken);

        persona.TipoDocumento = tipoDocumento;

        return _mapper.Map<PersonaDto>(persona);
    }
}