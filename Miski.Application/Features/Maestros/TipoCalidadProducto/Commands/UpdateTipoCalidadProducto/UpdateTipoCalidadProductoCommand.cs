using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Commands.UpdateTipoCalidadProducto;

public record UpdateTipoCalidadProductoCommand(int Id, UpdateTipoCalidadProductoDto TipoCalidadProducto) : IRequest<TipoCalidadProductoDto>;
