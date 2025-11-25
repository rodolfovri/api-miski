using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Permisos;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Permisos.Commands.CreateAccion;

public class CreateAccionHandler : IRequestHandler<CreateAccionCommand, AccionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAccionHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AccionDto> Handle(CreateAccionCommand request, CancellationToken cancellationToken)
    {
        // Validar que el código no exista
        var acciones = await _unitOfWork.Repository<Accion>().GetAllAsync(cancellationToken);
        var existe = acciones.Any(a => a.Codigo.Equals(request.Data.Codigo, StringComparison.OrdinalIgnoreCase));

        if (existe)
        {
            throw new ValidationException($"Ya existe una acción con el código '{request.Data.Codigo}'");
        }

        var nuevaAccion = new Accion
        {
            Nombre = request.Data.Nombre,
            Codigo = request.Data.Codigo,
            Icono = request.Data.Icono,
            Orden = request.Data.Orden,
            Estado = request.Data.Estado
        };

        await _unitOfWork.Repository<Accion>().AddAsync(nuevaAccion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AccionDto>(nuevaAccion);
    }
}
