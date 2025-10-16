using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.UnidadMedida.Queries.GetUnidadMedidas;

public record GetUnidadMedidasQuery(string? Nombre = null) : IRequest<List<UnidadMedidaDto>>;