using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.CompletarNegociacion;

public record CompletarNegociacionCommand(CompletarNegociacionDto Completar) : IRequest<NegociacionDto>;
