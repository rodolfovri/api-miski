using MediatR;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Application.Features.Ubicaciones.Queries.GetUbicacionById;

public record GetUbicacionByIdQuery(int Id) : IRequest<UbicacionDto>;