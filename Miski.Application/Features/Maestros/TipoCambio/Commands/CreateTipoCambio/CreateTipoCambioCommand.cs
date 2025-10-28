using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCambio.Commands.CreateTipoCambio;

public record CreateTipoCambioCommand(CreateTipoCambioDto TipoCambio) : IRequest<TipoCambioDto>;
