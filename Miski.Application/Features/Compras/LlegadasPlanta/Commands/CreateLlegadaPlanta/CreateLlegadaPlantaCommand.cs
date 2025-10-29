using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Commands.CreateLlegadaPlanta;

public record CreateLlegadaPlantaCommand(CreateLlegadaPlantaDto Data) : IRequest<CreateLlegadaPlantaResponseDto>;
