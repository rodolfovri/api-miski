using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.Commands.CreatePersona;

public class CreatePersonaHandler : IRequestHandler<CreatePersonaCommand, PersonaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePersonaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PersonaDto> Handle(CreatePersonaCommand request, CancellationToken cancellationToken)
    {
        // Validar que el tipo de documento existe
        var tipoDocumento = await _unitOfWork.Repository<TipoDocumento>()
            .GetByIdAsync(request.Persona.IdTipoDocumento, cancellationToken);

        if (tipoDocumento == null)
            throw new NotFoundException("TipoDocumento", request.Persona.IdTipoDocumento);

        // Verificar que no exista una persona con el mismo número de documento
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var personaExistente = personas.FirstOrDefault(p => 
            p.NumeroDocumento == request.Persona.NumeroDocumento);

        if (personaExistente != null)
        {
            throw new ValidationException("Ya existe una persona con este número de documento");
        }

        // Crear la persona
        var nuevaPersona = new Persona
        {
            IdTipoDocumento = request.Persona.IdTipoDocumento,
            NumeroDocumento = request.Persona.NumeroDocumento,
            Nombres = request.Persona.Nombres,
            Apellidos = request.Persona.Apellidos,
            Telefono = request.Persona.Telefono,
            Email = request.Persona.Email,
            Direccion = request.Persona.Direccion,
            Estado = request.Persona.Estado,
            FRegistro = DateTime.Now
        };

        await _unitOfWork.Repository<Persona>().AddAsync(nuevaPersona, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar el tipo de documento para el DTO
        nuevaPersona.TipoDocumento = tipoDocumento;

        return _mapper.Map<PersonaDto>(nuevaPersona);
    }
}