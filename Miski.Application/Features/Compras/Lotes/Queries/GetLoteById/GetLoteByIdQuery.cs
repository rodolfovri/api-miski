using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Lotes.Queries.GetLoteById;

public record GetLoteByIdQuery(int Id) : IRequest<LoteDto>;
