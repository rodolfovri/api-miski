using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Ubicaciones.Commands.RevocarUbicacion;

public class RevocarUbicacionHandler : IRequestHandler<RevocarUbicacionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public RevocarUbicacionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RevocarUbicacionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

        // Buscar la asignación
        var personaUbicaciones = await _unitOfWork.Repository<PersonaUbicacion>()
            .GetAllAsync(cancellationToken);

        var asignacion = personaUbicaciones.FirstOrDefault(pu => 
            pu.IdPersona == dto.IdPersona && pu.IdUbicacion == dto.IdUbicacion);

        if (asignacion == null)
        {
            throw new NotFoundException("PersonaUbicacion", $"Persona: {dto.IdPersona}, Ubicación: {dto.IdUbicacion}");
        }

        // Eliminar la asignación
        await _unitOfWork.Repository<PersonaUbicacion>()
            .DeleteAsync(asignacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
