using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Commands.CreateTipoCalidadProducto;

public record CreateTipoCalidadProductoCommand(CreateTipoCalidadProductoDto TipoCalidadProducto) : IRequest<TipoCalidadProductoDto>;
