using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Lotes.Queries.GetLotes;

public record GetLotesQuery(
    int? IdCompra,
    string? Codigo
) : IRequest<IEnumerable<LoteDto>>;
