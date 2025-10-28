using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCambio.Commands.UpdateTipoCambio;

public record UpdateTipoCambioCommand(int Id, UpdateTipoCambioDto TipoCambio) : IRequest<TipoCambioDto>;
