using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraPagos.Queries.GetHistorialPagos;

public record GetHistorialPagosQuery(int IdCompra) : IRequest<List<CompraPagoDto>>;
