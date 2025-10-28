using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Moneda.Commands.CreateMoneda;

public class CreateMonedaHandler : IRequestHandler<CreateMonedaCommand, MonedaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMonedaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MonedaDto> Handle(CreateMonedaCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Moneda;

        // Validar que el código no esté duplicado
        var monedas = await _unitOfWork.Repository<Domain.Entities.Moneda>()
            .GetAllAsync(cancellationToken);
        
        if (monedas.Any(m => m.Codigo.ToUpper() == dto.Codigo.ToUpper()))
            throw new ValidationException($"Ya existe una moneda con el código '{dto.Codigo}'");

        // Crear la moneda
        var moneda = new Domain.Entities.Moneda
        {
            Nombre = dto.Nombre.Trim(),
            Simbolo = dto.Simbolo.Trim(),
            Codigo = dto.Codigo.ToUpper().Trim()
        };

        await _unitOfWork.Repository<Domain.Entities.Moneda>().AddAsync(moneda, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MonedaDto>(moneda);
    }
}
