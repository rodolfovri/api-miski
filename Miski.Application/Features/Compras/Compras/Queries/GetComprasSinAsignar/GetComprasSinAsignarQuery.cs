using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Compras.Queries.GetComprasSinAsignar;

public record GetComprasSinAsignarQuery : IRequest<List<CompraDto>>;
