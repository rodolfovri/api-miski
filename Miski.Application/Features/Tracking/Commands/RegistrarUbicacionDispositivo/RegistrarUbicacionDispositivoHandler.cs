using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Tracking.Commands.RegistrarUbicacionDispositivo;

/// <summary>
/// Handler para registrar ubicación desde dispositivo sin JWT
/// Valida que el DeviceId exista y esté activo antes de guardar
/// </summary>
public class RegistrarUbicacionDispositivoHandler : IRequestHandler<RegistrarUbicacionDispositivoCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarUbicacionDispositivoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RegistrarUbicacionDispositivoCommand request, CancellationToken cancellationToken)
    {
        var data = request.Data;

        // Validar que el DeviceId exista y esté activo
        var dispositivos = await _unitOfWork.Repository<DispositivoPersona>().GetAllAsync(cancellationToken);
        var dispositivo = dispositivos.FirstOrDefault(d =>
            d.DeviceId == data.DeviceId &&
            d.Activo);

        if (dispositivo == null)
        {
            throw new ValidationException("Dispositivo no registrado o inactivo");
        }

        // Validar que IdPersona coincida con el dispositivo (seguridad)
        if (dispositivo.IdPersona != data.IdPersona)
        {
            throw new ValidationException("IdPersona no coincide con el dispositivo registrado");
        }

        // Desactivar la ubicación actual anterior (solo una puede ser actual)
        var trackings = await _unitOfWork.Repository<TrackingPersona>().GetAllAsync(cancellationToken);
        var trackingActual = trackings.FirstOrDefault(t => t.IdPersona == data.IdPersona && t.EsActual);
        if (trackingActual != null)
        {
            trackingActual.EsActual = false;
            trackingActual.FActualizacion = DateTime.UtcNow;
            await _unitOfWork.Repository<TrackingPersona>().UpdateAsync(trackingActual);
        }

        // Crear nuevo registro de tracking
        var nuevoTracking = new TrackingPersona
        {
            IdPersona = data.IdPersona,
            Latitud = data.Latitud,
            Longitud = data.Longitud,
            Precision = data.Precision,
            Velocidad = data.Velocidad,
            FRegistro = DateTime.UtcNow,
            FActualizacion = DateTime.UtcNow,
            Estado = "ACTIVO",
            EsActual = true
        };

        await _unitOfWork.Repository<TrackingPersona>().AddAsync(nuevoTracking);

        // Actualizar última actividad del dispositivo
        dispositivo.FUltimaActividad = DateTime.UtcNow;
        await _unitOfWork.Repository<DispositivoPersona>().UpdateAsync(dispositivo);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Aquí puedes agregar SignalR para notificar en tiempo real
        // await _hubContext.Clients.All.SendAsync("UbicacionActualizada", ...);

        return Unit.Value;
    }
}
