using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.AprobarNegociacionIngeniero;

public record AprobarNegociacionIngenieroCommand(AprobarNegociacionIngenieroDto Aprobacion) : IRequest<NegociacionDto>;
