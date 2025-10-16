using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.UpdateNegociacion;

public record UpdateNegociacionCommand(int Id, UpdateNegociacionDto Negociacion) : IRequest<NegociacionDto>;
