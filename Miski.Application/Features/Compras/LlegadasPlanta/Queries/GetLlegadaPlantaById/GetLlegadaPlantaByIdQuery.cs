using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetLlegadaPlantaById;

public record GetLlegadaPlantaByIdQuery(int Id) : IRequest<LlegadaPlantaDto>;
