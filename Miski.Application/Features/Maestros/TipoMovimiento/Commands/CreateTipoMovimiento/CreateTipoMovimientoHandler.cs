using AutoMapper;
using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Commands.CreateTipoMovimiento;

public class CreateTipoMovimientoHandler : IRequestHandler<CreateTipoMovimientoCommand, TipoMovimientoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTipoMovimientoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TipoMovimientoDto> Handle(CreateTipoMovimientoCommand request, CancellationToken cancellationToken)
    {
        var tipoMovimiento = new Domain.Entities.TipoMovimiento
        {
            TipoOperacion = request.TipoMovimientoData.TipoOperacion.ToUpper(),
            Descripcion = request.TipoMovimientoData.Descripcion,
            Estado = request.TipoMovimientoData.Estado ?? "ACTIVO"
        };

        await _unitOfWork.Repository<Domain.Entities.TipoMovimiento>().AddAsync(tipoMovimiento);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TipoMovimientoDto>(tipoMovimiento);
    }
}
