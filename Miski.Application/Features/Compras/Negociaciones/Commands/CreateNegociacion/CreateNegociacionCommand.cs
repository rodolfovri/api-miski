using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.CreateNegociacion;

public record CreateNegociacionCommand(CreateNegociacionDto Negociacion) : IRequest<NegociacionDto>;