using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.VariedadProducto.Queries.GetVariedadById;

public record GetVariedadProductoByIdQuery(int Id) : IRequest<VariedadProductoDto>;
