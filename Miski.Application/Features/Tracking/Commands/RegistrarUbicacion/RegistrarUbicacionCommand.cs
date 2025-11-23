using MediatR;
using Miski.Shared.DTOs.Tracking;

namespace Miski.Application.Features.Tracking.Commands.RegistrarUbicacion;

/// <summary>
/// Command para registrar ubicación con usuario autenticado
/// Usado cuando el usuario está activo en la app
/// </summary>
public class RegistrarUbicacionCommand : IRequest<Unit>
{
    public int IdPersona { get; set; } // Se obtiene del JWT
    public RegistrarUbicacionDto Data { get; set; } = null!;
}
