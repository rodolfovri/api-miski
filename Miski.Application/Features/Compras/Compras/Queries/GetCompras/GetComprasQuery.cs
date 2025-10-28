using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Compras.Queries.GetCompras;

public record GetComprasQuery(string? Estado = null, int? IdNegociacion = null) : IRequest<List<CompraDto>>;
