using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Compras.Queries.GetCompraById;

public record GetCompraByIdQuery(int Id) : IRequest<CompraDto>;
