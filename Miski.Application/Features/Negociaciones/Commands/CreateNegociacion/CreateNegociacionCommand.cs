using MediatR;
using Miski.Shared.DTOs;

namespace Miski.Application.Features.Negociaciones.Commands.CreateNegociacion;

public record CreateNegociacionCommand(CreateNegociacionDto Negociacion) : IRequest<NegociacionDto>;