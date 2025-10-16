using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.UnidadMedida.Commands.CreateUnidadMedida;

public record CreateUnidadMedidaCommand(CreateUnidadMedidaDto UnidadMedida) : IRequest<UnidadMedidaDto>;