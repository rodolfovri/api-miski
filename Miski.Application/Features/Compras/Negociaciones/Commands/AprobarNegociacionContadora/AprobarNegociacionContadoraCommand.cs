using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.AprobarNegociacionContadora;

public record AprobarNegociacionContadoraCommand(AprobarNegociacionContadoraDto Aprobacion) : IRequest<NegociacionDto>;
