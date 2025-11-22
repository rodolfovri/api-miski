using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Cargo.Queries.GetCargosByPersona;

public class GetCargosByPersonaHandler : IRequestHandler<GetCargosByPersonaQuery, List<PersonaCargoDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCargosByPersonaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PersonaCargoDto>> Handle(GetCargosByPersonaQuery request, CancellationToken cancellationToken)
    {
        // Validar que la persona existe
        var persona = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(request.IdPersona, cancellationToken);

        if (persona == null)
        {
            throw new NotFoundException(nameof(Persona), request.IdPersona);
        }

        // Obtener todos los PersonaCargo
        var personaCargos = await _unitOfWork.Repository<PersonaCargo>()
            .GetAllAsync(cancellationToken);

        // Filtrar por persona
        var cargosFiltrados = personaCargos.Where(pc => pc.IdPersona == request.IdPersona);

        // Aplicar filtro de solo actuales si se especifica
        if (request.SoloActuales.HasValue && request.SoloActuales.Value)
        {
            cargosFiltrados = cargosFiltrados.Where(pc => pc.EsActual);
        }

        var listaFinal = cargosFiltrados.ToList();

        // Obtener los cargos para el mapeo
        var cargos = await _unitOfWork.Repository<Domain.Entities.Cargo>()
            .GetAllAsync(cancellationToken);

        // Mapear a DTOs
        var result = new List<PersonaCargoDto>();
        foreach (var pc in listaFinal)
        {
            var cargo = cargos.FirstOrDefault(c => c.IdCargo == pc.IdCargo);
            
            result.Add(new PersonaCargoDto
            {
                IdPersonaCargo = pc.IdPersonaCargo,
                IdPersona = pc.IdPersona,
                IdCargo = pc.IdCargo,
                FechaInicio = pc.FechaInicio,
                FechaFin = pc.FechaFin,
                EsActual = pc.EsActual,
                ObservacionAsignacion = pc.ObservacionAsignacion,
                MotivoRevocacion = pc.MotivoRevocacion,
                PersonaNombre = $"{persona.Nombres} {persona.Apellidos}",
                CargoNombre = cargo?.Nombre
            });
        }

        return result.OrderByDescending(x => x.EsActual).ThenByDescending(x => x.FechaInicio).ToList();
    }
}
