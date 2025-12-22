using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.AprobarEvidenciasIngeniero;

public record AprobarEvidenciasIngenieroCommand(AprobarEvidenciasIngenieroDto Aprobacion) : IRequest<NegociacionDto>;
