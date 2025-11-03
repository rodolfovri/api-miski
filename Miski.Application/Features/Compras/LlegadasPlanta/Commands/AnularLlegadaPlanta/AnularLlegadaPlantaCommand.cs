using MediatR;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Commands.AnularLlegadaPlanta;

public record AnularLlegadaPlantaCommand(
    int IdLlegadaPlanta,
    int IdUsuarioAnulacion,
    string MotivoAnulacion
) : IRequest<Unit>;
