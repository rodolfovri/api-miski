using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.UnidadMedida.Queries.GetUnidadMedidaById;

public record GetUnidadMedidaByIdQuery(int Id) : IRequest<UnidadMedidaDto>;