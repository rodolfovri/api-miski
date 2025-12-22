using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.RechazarEvidenciasIngeniero;

public record RechazarEvidenciasIngenieroCommand(RechazarEvidenciasIngenieroDto Rechazo) : IRequest<NegociacionDto>;
