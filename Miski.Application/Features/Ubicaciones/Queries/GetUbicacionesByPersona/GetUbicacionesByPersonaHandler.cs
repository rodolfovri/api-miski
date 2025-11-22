using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Application.Features.Ubicaciones.Queries.GetUbicacionesByPersona;

public class GetUbicacionesByPersonaHandler : IRequestHandler<GetUbicacionesByPersonaQuery, List<PersonaUbicacionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUbicacionesByPersonaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PersonaUbicacionDto>> Handle(GetUbicacionesByPersonaQuery request, CancellationToken cancellationToken)
    {
        // Obtener todas las relaciones PersonaUbicacion
        var personaUbicaciones = await _unitOfWork.Repository<PersonaUbicacion>()
            .GetAllAsync(cancellationToken);

        // Filtrar por persona
        var ubicacionesDePersona = personaUbicaciones
            .Where(pu => pu.IdPersona == request.IdPersona)
            .ToList();

        // Obtener todas las ubicaciones para mapear
        var ubicaciones = await _unitOfWork.Repository<Ubicacion>()
            .GetAllAsync(cancellationToken);

        // Crear los DTOs con información de la ubicación
        var resultado = ubicacionesDePersona.Select(pu =>
        {
            var ubicacion = ubicaciones.FirstOrDefault(u => u.IdUbicacion == pu.IdUbicacion);

            return new PersonaUbicacionDto
            {
                IdPersonaUbicacion = pu.IdPersonaUbicacion,
                IdPersona = pu.IdPersona,
                IdUbicacion = pu.IdUbicacion,
                FRegistro = pu.FRegistro,
                UbicacionNombre = ubicacion?.Nombre,
                UbicacionTipo = ubicacion?.Tipo,
                UbicacionDireccion = ubicacion?.Direccion,
                UbicacionEstado = ubicacion?.Estado
            };
        }).ToList();

        return resultado;
    }
}
