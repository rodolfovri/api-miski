using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.Commands.UpdatePersona;

public class UpdatePersonaHandler : IRequestHandler<UpdatePersonaCommand, PersonaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePersonaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PersonaDto> Handle(UpdatePersonaCommand request, CancellationToken cancellationToken)
    {
        // Buscar la persona
        var persona = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (persona == null)
            throw new NotFoundException("Persona", request.Id);

        // Validar que el tipo de documento existe
        var tipoDocumento = await _unitOfWork.Repository<TipoDocumento>()
            .GetByIdAsync(request.Persona.IdTipoDocumento, cancellationToken);

        if (tipoDocumento == null)
            throw new NotFoundException("TipoDocumento", request.Persona.IdTipoDocumento);

        // Verificar que no exista otra persona con el mismo n�mero de documento
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var personaExistente = personas.FirstOrDefault(p => 
            p.NumeroDocumento == request.Persona.NumeroDocumento && 
            p.IdPersona != request.Id);

        if (personaExistente != null)
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                { "NumeroDocumento", new[] { "Ya existe otra persona con este n�mero de documento" } }
            });
        }

        // Actualizar la persona
        persona.IdTipoDocumento = request.Persona.IdTipoDocumento;
        persona.NumeroDocumento = request.Persona.NumeroDocumento;
        persona.Nombres = request.Persona.Nombres;
        persona.Apellidos = request.Persona.Apellidos;
        persona.Telefono = request.Persona.Telefono;
        persona.Email = request.Persona.Email;
        persona.Direccion = request.Persona.Direccion;
        
        if (!string.IsNullOrEmpty(request.Persona.Estado))
        {
            persona.Estado = request.Persona.Estado;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar el tipo de documento para el DTO
        persona.TipoDocumento = tipoDocumento;

        return _mapper.Map<PersonaDto>(persona);
    }
}