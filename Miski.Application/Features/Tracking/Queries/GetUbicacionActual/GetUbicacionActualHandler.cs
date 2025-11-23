using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Tracking;

namespace Miski.Application.Features.Tracking.Queries.GetUbicacionActual;

public class GetUbicacionActualHandler : IRequestHandler<GetUbicacionActualQuery, TrackingResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUbicacionActualHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TrackingResponseDto?> Handle(GetUbicacionActualQuery request, CancellationToken cancellationToken)
    {
        var trackings = await _unitOfWork.Repository<TrackingPersona>().GetAllAsync(cancellationToken);
        var trackingActual = trackings.FirstOrDefault(t =>
            t.IdPersona == request.IdPersona &&
            t.EsActual &&
            t.Estado == "ACTIVO");

        if (trackingActual == null)
            return null;

        // Cargar datos de la persona
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var persona = personas.FirstOrDefault(p => p.IdPersona == request.IdPersona);

        return new TrackingResponseDto
        {
            IdTracking = trackingActual.IdTracking,
            IdPersona = trackingActual.IdPersona,
            NombreCompleto = persona != null ? $"{persona.Nombres} {persona.Apellidos}" : "",
            NumeroDocumento = persona?.NumeroDocumento ?? "",
            Latitud = trackingActual.Latitud,
            Longitud = trackingActual.Longitud,
            Precision = trackingActual.Precision,
            Velocidad = trackingActual.Velocidad,
            FRegistro = trackingActual.FRegistro,
            EsActual = trackingActual.EsActual,
            Estado = trackingActual.Estado
        };
    }
}
