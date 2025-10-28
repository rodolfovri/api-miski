using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Moneda.Commands.UpdateMoneda;

public record UpdateMonedaCommand(int Id, UpdateMonedaDto Moneda) : IRequest<MonedaDto>;
