using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Cargo.Commands.RevocarCargo;

public class RevocarCargoHandler : IRequestHandler<RevocarCargoCommand, PersonaCargoDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public RevocarCargoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PersonaCargoDto> Handle(RevocarCargoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

        // Buscar la asignación
        var personaCargo = await _unitOfWork.Repository<PersonaCargo>()
            .GetByIdAsync(dto.IdPersonaCargo, cancellationToken);

        if (personaCargo == null)
        {
            throw new NotFoundException(nameof(PersonaCargo), dto.IdPersonaCargo);
        }

        // Validar que el cargo esté activo
        if (!personaCargo.EsActual)
        {
            throw new ValidationException("El cargo ya ha sido revocado anteriormente");
        }

        // Validar que la fecha fin sea posterior a la fecha inicio
        if (dto.FechaFin < personaCargo.FechaInicio)
        {
            throw new ValidationException("La fecha de fin no puede ser anterior a la fecha de inicio");
        }

        // Revocar el cargo
        personaCargo.EsActual = false;
        personaCargo.FechaFin = dto.FechaFin;
        personaCargo.MotivoRevocacion = dto.MotivoRevocacion;

        await _unitOfWork.Repository<PersonaCargo>()
            .UpdateAsync(personaCargo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar las relaciones para el DTO
        var persona = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(personaCargo.IdPersona, cancellationToken);

        var cargo = await _unitOfWork.Repository<Domain.Entities.Cargo>()
            .GetByIdAsync(personaCargo.IdCargo, cancellationToken);

        // Retornar el DTO
        return new PersonaCargoDto
        {
            IdPersonaCargo = personaCargo.IdPersonaCargo,
            IdPersona = personaCargo.IdPersona,
            IdCargo = personaCargo.IdCargo,
            FechaInicio = personaCargo.FechaInicio,
            FechaFin = personaCargo.FechaFin,
            EsActual = personaCargo.EsActual,
            ObservacionAsignacion = personaCargo.ObservacionAsignacion,
            MotivoRevocacion = personaCargo.MotivoRevocacion,
            PersonaNombre = persona != null ? $"{persona.Nombres} {persona.Apellidos}" : null,
            CargoNombre = cargo?.Nombre
        };
    }
}
