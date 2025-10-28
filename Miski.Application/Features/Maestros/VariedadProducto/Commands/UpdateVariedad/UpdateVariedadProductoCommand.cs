using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.VariedadProducto.Commands.UpdateVariedad;

public record UpdateVariedadProductoCommand(int Id, UpdateVariedadProductoDto Variedad) : IRequest<VariedadProductoDto>;
