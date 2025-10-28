using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Lotes.Commands.UpdateLote;

public record UpdateLoteCommand(int Id, UpdateLoteDto Lote) : IRequest<LoteDto>;
