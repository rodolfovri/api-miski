using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Tracking;

namespace Miski.Application.Features.Tracking.Queries.GetUbicacionesActivas;

public class GetUbicacionesActivasHandler : IRequestHandler<GetUbicacionesActivasQuery, List<TrackingResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUbicacionesActivasHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TrackingResponseDto>> Handle(GetUbicacionesActivasQuery request, CancellationToken cancellationToken)
    {
        var trackings = await _unitOfWork.Repository<TrackingPersona>().GetAllAsync(cancellationToken);
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);

        // Obtener solo las ubicaciones actuales y activas
        var trackingsActivos = trackings.Where(t =>
            t.EsActual &&
            t.Estado == "ACTIVO")
            .OrderByDescending(t => t.FRegistro)
            .ToList();

        var resultado = trackingsActivos.Select(t =>
        {
            var persona = personas.FirstOrDefault(p => p.IdPersona == t.IdPersona);
            return new TrackingResponseDto
            {
                IdTracking = t.IdTracking,
                IdPersona = t.IdPersona,
                NombreCompleto = persona != null ? $"{persona.Nombres} {persona.Apellidos}" : "",
                NumeroDocumento = persona?.NumeroDocumento ?? "",
                Latitud = t.Latitud,
                Longitud = t.Longitud,
                Precision = t.Precision,
                Velocidad = t.Velocidad,
                FRegistro = t.FRegistro,
                EsActual = t.EsActual,
                Estado = t.Estado
            };
        }).ToList();

        return resultado;
    }
}
