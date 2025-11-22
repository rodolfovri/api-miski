using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;
using Miski.Application.Services;

namespace Miski.Application.Features.Maestros.VariedadProducto.Commands.CreateVariedad;

public class CreateVariedadProductoHandler : IRequestHandler<CreateVariedadProductoCommand, VariedadProductoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;

    public CreateVariedadProductoHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    public async Task<VariedadProductoDto> Handle(CreateVariedadProductoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Variedad;

        // Validar que el producto existe
        var producto = await _unitOfWork.Repository<Producto>()
            .GetByIdAsync(dto.IdProducto, cancellationToken);
        
        if (producto == null)
            throw new NotFoundException("Producto", dto.IdProducto);

        // Validar que la unidad de medida existe
        var unidadMedida = await _unitOfWork.Repository<Domain.Entities.UnidadMedida>()
            .GetByIdAsync(dto.IdUnidadMedida, cancellationToken);
        
        if (unidadMedida == null)
            throw new NotFoundException("UnidadMedida", dto.IdUnidadMedida);

        // Validar que el código no exista
        var variedades = await _unitOfWork.Repository<Domain.Entities.VariedadProducto>().GetAllAsync(cancellationToken);
        if (variedades.Any(v => v.Codigo == dto.Codigo))
            throw new ValidationException($"Ya existe una variedad con el código {dto.Codigo}");

        // Guardar ficha técnica (PDF) si se proporciona
        string? fichaTecnicaUrl = null;
        if (dto.FichaTecnica != null)
        {
            fichaTecnicaUrl = await _fileStorageService.SaveFileAsync(
                dto.FichaTecnica, 
                "variedad-productos/fichas-tecnicas", 
                cancellationToken);
        }

        var variedad = new Domain.Entities.VariedadProducto
        {
            IdProducto = dto.IdProducto,
            IdUnidadMedida = dto.IdUnidadMedida,
            Codigo = dto.Codigo,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Estado = dto.Estado,
            FichaTecnica = fichaTecnicaUrl,
            FRegistro = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Domain.Entities.VariedadProducto>().AddAsync(variedad, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        variedad.Producto = producto;
        variedad.UnidadMedida = unidadMedida;

        return _mapper.Map<VariedadProductoDto>(variedad);
    }
}
