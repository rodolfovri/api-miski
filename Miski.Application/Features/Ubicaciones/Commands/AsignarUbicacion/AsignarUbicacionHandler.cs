using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Ubicaciones;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Ubicaciones.Commands.AsignarUbicacion;

public class AsignarUbicacionHandler : IRequestHandler<AsignarUbicacionCommand, PersonaUbicacionDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public AsignarUbicacionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PersonaUbicacionDto> Handle(AsignarUbicacionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

        // Validar que la persona existe
        var persona = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(dto.IdPersona, cancellationToken);

        if (persona == null)
        {
            throw new NotFoundException(nameof(Persona), dto.IdPersona);
        }

        // Validar que la ubicación existe
        var ubicacion = await _unitOfWork.Repository<Ubicacion>()
            .GetByIdAsync(dto.IdUbicacion, cancellationToken);

        if (ubicacion == null)
        {
            throw new NotFoundException(nameof(Ubicacion), dto.IdUbicacion);
        }

        // Verificar que no exista ya la asignación
        var personaUbicaciones = await _unitOfWork.Repository<PersonaUbicacion>()
            .GetAllAsync(cancellationToken);

        var existeAsignacion = personaUbicaciones.Any(pu => 
            pu.IdPersona == dto.IdPersona && pu.IdUbicacion == dto.IdUbicacion);

        if (existeAsignacion)
        {
            throw new ValidationException("La persona ya tiene asignada esta ubicación");
        }

        // Crear la nueva asignación
        var nuevaAsignacion = new PersonaUbicacion
        {
            IdPersona = dto.IdPersona,
            IdUbicacion = dto.IdUbicacion,
            FRegistro = DateTime.UtcNow
        };

        await _unitOfWork.Repository<PersonaUbicacion>()
            .AddAsync(nuevaAsignacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el DTO
        return new PersonaUbicacionDto
        {
            IdPersonaUbicacion = nuevaAsignacion.IdPersonaUbicacion,
            IdPersona = nuevaAsignacion.IdPersona,
            IdUbicacion = nuevaAsignacion.IdUbicacion,
            FRegistro = nuevaAsignacion.FRegistro,
            UbicacionNombre = ubicacion.Nombre,
            UbicacionTipo = ubicacion.Tipo,
            UbicacionDireccion = ubicacion.Direccion,
            UbicacionEstado = ubicacion.Estado
        };
    }
}
