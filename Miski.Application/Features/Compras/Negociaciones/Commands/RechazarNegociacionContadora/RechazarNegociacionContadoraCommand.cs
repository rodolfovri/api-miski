using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.RechazarNegociacionContadora;

public record RechazarNegociacionContadoraCommand(RechazarNegociacionContadoraDto Rechazo) : IRequest<NegociacionDto>;
