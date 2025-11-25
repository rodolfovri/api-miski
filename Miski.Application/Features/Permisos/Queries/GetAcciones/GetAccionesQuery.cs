using MediatR;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Queries.GetAcciones;

/// <summary>
/// Query para obtener todas las acciones disponibles
/// </summary>
public class GetAccionesQuery : IRequest<List<AccionDto>>
{
}
