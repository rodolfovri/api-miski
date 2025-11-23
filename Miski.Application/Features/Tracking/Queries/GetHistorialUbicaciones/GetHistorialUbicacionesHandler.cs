using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Tracking;

namespace Miski.Application.Features.Tracking.Queries.GetHistorialUbicaciones;

public class GetHistorialUbicacionesHandler : IRequestHandler<GetHistorialUbicacionesQuery, List<TrackingResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetHistorialUbicacionesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TrackingResponseDto>> Handle(GetHistorialUbicacionesQuery request, CancellationToken cancellationToken)
    {
        var trackings = await _unitOfWork.Repository<TrackingPersona>().GetAllAsync(cancellationToken);
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);

        var query = trackings.Where(t =>
            t.IdPersona == request.IdPersona &&
            t.Estado == "ACTIVO");

        // Filtrar por fechas si se proporcionan
        if (request.FechaInicio.HasValue)
        {
            query = query.Where(t => t.FRegistro >= request.FechaInicio.Value);
        }

        if (request.FechaFin.HasValue)
        {
            query = query.Where(t => t.FRegistro <= request.FechaFin.Value);
        }

        // Ordenar por fecha descendente y limitar resultados
        var resultado = query
            .OrderByDescending(t => t.FRegistro)
            .Take(request.Limite ?? 100)
            .ToList();

        var persona = personas.FirstOrDefault(p => p.IdPersona == request.IdPersona);
        var nombreCompleto = persona != null ? $"{persona.Nombres} {persona.Apellidos}" : "";
        var numeroDocumento = persona?.NumeroDocumento ?? "";

        return resultado.Select(t => new TrackingResponseDto
        {
            IdTracking = t.IdTracking,
            IdPersona = t.IdPersona,
            NombreCompleto = nombreCompleto,
            NumeroDocumento = numeroDocumento,
            Latitud = t.Latitud,
            Longitud = t.Longitud,
            Precision = t.Precision,
            Velocidad = t.Velocidad,
            FRegistro = t.FRegistro,
            EsActual = t.EsActual,
            Estado = t.Estado
        }).ToList();
    }
}
