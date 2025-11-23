using MediatR;
using Miski.Shared.DTOs.Tracking;

namespace Miski.Application.Features.Tracking.Commands.RegistrarUbicacionDispositivo;

/// <summary>
/// Command para registrar ubicación desde dispositivo sin autenticación JWT
/// Usado para tracking en background cuando la app está cerrada
/// </summary>
public class RegistrarUbicacionDispositivoCommand : IRequest<Unit>
{
    public UbicacionDispositivoDto Data { get; set; } = null!;
}
