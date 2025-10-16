using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.UnidadMedida.Commands.UpdateUnidadMedida;

public record UpdateUnidadMedidaCommand(int Id, UpdateUnidadMedidaDto UnidadMedida) : IRequest<UnidadMedidaDto>;