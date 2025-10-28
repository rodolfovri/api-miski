using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.VariedadProducto.Commands.CreateVariedad;

public record CreateVariedadProductoCommand(CreateVariedadProductoDto Variedad) : IRequest<VariedadProductoDto>;
