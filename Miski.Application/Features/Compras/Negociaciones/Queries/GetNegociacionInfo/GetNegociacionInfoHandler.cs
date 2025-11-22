using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociacionInfo;

public class GetNegociacionInfoHandler : IRequestHandler<GetNegociacionInfoQuery, NegociacionInfoDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetNegociacionInfoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NegociacionInfoDto> Handle(GetNegociacionInfoQuery request, CancellationToken cancellationToken)
    {
        // Obtener la negociación
        var negociacion = await _unitOfWork.Repository<Negociacion>()
            .GetByIdAsync(request.IdNegociacion, cancellationToken);

        if (negociacion == null)
            throw new NotFoundException("Negociacion", request.IdNegociacion);

        // Retornar la información
        return new NegociacionInfoDto
        {
            IdNegociacion = negociacion.IdNegociacion,
            SacosTotales = negociacion.SacosTotales ?? 0,
            PesoTotal = negociacion.PesoTotal ?? 0,
            PesoPorSaco = negociacion.PesoPorSaco ?? 0,
            PrecioUnitario = negociacion.PrecioUnitario ?? 0,
            MontoTotalPago = negociacion.MontoTotalPago ?? 0
        };
    }
}
