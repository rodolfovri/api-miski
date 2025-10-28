using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.RechazarNegociacionIngeniero;

public record RechazarNegociacionIngenieroCommand(RechazarNegociacionIngenieroDto Rechazo) : IRequest<NegociacionDto>;
