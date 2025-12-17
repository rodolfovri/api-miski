using AutoMapper;
using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Commands.UpdateTipoMovimiento;

public class UpdateTipoMovimientoHandler : IRequestHandler<UpdateTipoMovimientoCommand, TipoMovimientoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTipoMovimientoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TipoMovimientoDto> Handle(UpdateTipoMovimientoCommand request, CancellationToken cancellationToken)
    {
        var tipoMovimiento = await _unitOfWork.Repository<Domain.Entities.TipoMovimiento>()
            .GetByIdAsync(request.TipoMovimientoData.IdTipoMovimiento, cancellationToken);

        if (tipoMovimiento == null)
        {
            throw new NotFoundException("TipoMovimiento", request.TipoMovimientoData.IdTipoMovimiento);
        }

        tipoMovimiento.TipoOperacion = request.TipoMovimientoData.TipoOperacion.ToUpper();
        tipoMovimiento.Descripcion = request.TipoMovimientoData.Descripcion;
        tipoMovimiento.Estado = request.TipoMovimientoData.Estado;

        await _unitOfWork.Repository<Domain.Entities.TipoMovimiento>().UpdateAsync(tipoMovimiento);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TipoMovimientoDto>(tipoMovimiento);
    }
}
