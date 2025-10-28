using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Lotes.Commands.CreateLote;

public record CreateLoteCommand(CreateLoteDto Lote) : IRequest<LoteDto>;
