using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Moneda.Commands.CreateMoneda;

public record CreateMonedaCommand(CreateMonedaDto Moneda) : IRequest<MonedaDto>;
