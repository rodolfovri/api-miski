using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Moneda.Commands.UpdateMoneda;

public class UpdateMonedaHandler : IRequestHandler<UpdateMonedaCommand, MonedaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMonedaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MonedaDto> Handle(UpdateMonedaCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Moneda;

        var moneda = await _unitOfWork.Repository<Domain.Entities.Moneda>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (moneda == null)
            throw new NotFoundException("Moneda", request.Id);

        // Validar que el código no esté duplicado (excepto el actual)
        var monedas = await _unitOfWork.Repository<Domain.Entities.Moneda>()
            .GetAllAsync(cancellationToken);
        
        var codigoDuplicado = monedas
            .Any(m => m.Codigo.ToUpper() == dto.Codigo.ToUpper() && m.IdMoneda != moneda.IdMoneda);
        
        if (codigoDuplicado)
            throw new ValidationException($"Ya existe una moneda con el código '{dto.Codigo}'");

        // Actualizar la moneda
        moneda.Nombre = dto.Nombre.Trim();
        moneda.Simbolo = dto.Simbolo.Trim();
        moneda.Codigo = dto.Codigo.ToUpper().Trim();

        await _unitOfWork.Repository<Domain.Entities.Moneda>().UpdateAsync(moneda, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MonedaDto>(moneda);
    }
}
