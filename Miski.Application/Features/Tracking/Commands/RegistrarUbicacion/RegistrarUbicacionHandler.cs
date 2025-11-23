using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Tracking.Commands.RegistrarUbicacion;

/// <summary>
/// Handler para registrar ubicación con usuario autenticado
/// El IdPersona viene del JWT del usuario logueado
/// </summary>
public class RegistrarUbicacionHandler : IRequestHandler<RegistrarUbicacionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarUbicacionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RegistrarUbicacionCommand request, CancellationToken cancellationToken)
    {
        // Validar que la persona existe
        var persona = await _unitOfWork.Repository<Persona>().GetByIdAsync(request.IdPersona, cancellationToken);
        if (persona == null)
        {
            throw new NotFoundException("Persona", request.IdPersona);
        }

        // Desactivar la ubicación actual anterior (solo una puede ser actual)
        var trackings = await _unitOfWork.Repository<TrackingPersona>().GetAllAsync(cancellationToken);
        var trackingActual = trackings.FirstOrDefault(t => t.IdPersona == request.IdPersona && t.EsActual);
        if (trackingActual != null)
        {
            trackingActual.EsActual = false;
            trackingActual.FActualizacion = DateTime.UtcNow;
            await _unitOfWork.Repository<TrackingPersona>().UpdateAsync(trackingActual);
        }

        // Crear nuevo registro de tracking
        var nuevoTracking = new TrackingPersona
        {
            IdPersona = request.IdPersona,
            Latitud = request.Data.Latitud,
            Longitud = request.Data.Longitud,
            Precision = request.Data.Precision,
            Velocidad = request.Data.Velocidad,
            FRegistro = DateTime.UtcNow,
            FActualizacion = DateTime.UtcNow,
            Estado = "ACTIVO",
            EsActual = true
        };

        await _unitOfWork.Repository<TrackingPersona>().AddAsync(nuevoTracking);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Aquí puedes agregar SignalR para notificar en tiempo real
        // await _hubContext.Clients.All.SendAsync("UbicacionActualizada", ...);

        return Unit.Value;
    }
}
