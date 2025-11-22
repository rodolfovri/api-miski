using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Cargo.Commands.AsignarCargo;

public class AsignarCargoHandler : IRequestHandler<AsignarCargoCommand, PersonaCargoDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public AsignarCargoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PersonaCargoDto> Handle(AsignarCargoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

        // Validar que la persona existe
        var persona = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(dto.IdPersona, cancellationToken);

        if (persona == null)
        {
            throw new NotFoundException(nameof(Persona), dto.IdPersona);
        }

        // Validar que el cargo existe
        var cargo = await _unitOfWork.Repository<Domain.Entities.Cargo>()
            .GetByIdAsync(dto.IdCargo, cancellationToken);

        if (cargo == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Cargo), dto.IdCargo);
        }

        // Desactivar cargos actuales de la persona si los tiene
        var personaCargos = await _unitOfWork.Repository<PersonaCargo>()
            .GetAllAsync(cancellationToken);

        var cargosActuales = personaCargos.Where(pc => 
            pc.IdPersona == dto.IdPersona && pc.EsActual).ToList();

        foreach (var cargoActual in cargosActuales)
        {
            cargoActual.EsActual = false;
            cargoActual.FechaFin = dto.FechaInicio.AddDays(-1);
            await _unitOfWork.Repository<PersonaCargo>()
                .UpdateAsync(cargoActual, cancellationToken);
        }

        // Crear la nueva asignación
        var nuevaAsignacion = new PersonaCargo
        {
            IdPersona = dto.IdPersona,
            IdCargo = dto.IdCargo,
            FechaInicio = dto.FechaInicio,
            EsActual = true,
            ObservacionAsignacion = dto.ObservacionAsignacion
        };

        await _unitOfWork.Repository<PersonaCargo>()
            .AddAsync(nuevaAsignacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar el DTO
        return new PersonaCargoDto
        {
            IdPersonaCargo = nuevaAsignacion.IdPersonaCargo,
            IdPersona = nuevaAsignacion.IdPersona,
            IdCargo = nuevaAsignacion.IdCargo,
            FechaInicio = nuevaAsignacion.FechaInicio,
            FechaFin = nuevaAsignacion.FechaFin,
            EsActual = nuevaAsignacion.EsActual,
            ObservacionAsignacion = nuevaAsignacion.ObservacionAsignacion,
            MotivoRevocacion = nuevaAsignacion.MotivoRevocacion,
            PersonaNombre = $"{persona.Nombres} {persona.Apellidos}",
            CargoNombre = cargo.Nombre
        };
    }
}
