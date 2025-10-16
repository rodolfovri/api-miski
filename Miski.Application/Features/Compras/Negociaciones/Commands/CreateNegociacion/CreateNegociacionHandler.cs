using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;
using Miski.Application.Services;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.CreateNegociacion;

public class CreateNegociacionHandler : IRequestHandler<CreateNegociacionCommand, NegociacionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;

    public CreateNegociacionHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    public async Task<NegociacionDto> Handle(CreateNegociacionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Negociacion;

        // Validar que el proveedor existe si se proporciona
        if (dto.IdProveedor.HasValue)
        {
            var proveedor = await _unitOfWork.Repository<Persona>()
                .GetByIdAsync(dto.IdProveedor.Value, cancellationToken);
            
            if (proveedor == null)
                throw new NotFoundException("Proveedor", dto.IdProveedor.Value);
        }

        // Validar que el comisionista existe
        var comisionista = await _unitOfWork.Repository<Persona>()
            .GetByIdAsync(dto.IdComisionista, cancellationToken);
        
        if (comisionista == null)
            throw new NotFoundException("Comisionista", dto.IdComisionista);

        // Validar que el producto existe si se proporciona
        if (dto.IdProducto.HasValue)
        {
            var producto = await _unitOfWork.Repository<Producto>()
                .GetByIdAsync(dto.IdProducto.Value, cancellationToken);
            
            if (producto == null)
                throw new NotFoundException("Producto", dto.IdProducto.Value);
        }

        // Validar que las fotos estén presentes
        if (dto.FotoCalidadProducto == null || dto.FotoDniFrontal == null || dto.FotoDniPosterior == null)
        {
            throw new ValidationException("Las tres fotos son obligatorias: Calidad del Producto, DNI Frontal y DNI Posterior");
        }

        // Guardar las fotos
        var fotoCalidadUrl = await _fileStorageService.SaveFileAsync(dto.FotoCalidadProducto, "negociaciones/calidad", cancellationToken);
        var fotoDniFrontalUrl = await _fileStorageService.SaveFileAsync(dto.FotoDniFrontal, "negociaciones/dni", cancellationToken);
        var fotoDniPosteriorUrl = await _fileStorageService.SaveFileAsync(dto.FotoDniPosterior, "negociaciones/dni", cancellationToken);

        // Crear la negociación
        var negociacion = new Negociacion
        {
            IdProveedor = dto.IdProveedor,
            IdComisionista = dto.IdComisionista,
            IdProducto = dto.IdProducto,
            PesoTotal = dto.PesoTotal,
            SacosTotales = dto.SacosTotales,
            PrecioUnitario = dto.PrecioUnitario,
            NroCuentaRuc = dto.NroCuentaRuc,
            FotoCalidadProducto = fotoCalidadUrl,
            FotoDniFrontal = fotoDniFrontalUrl,
            FotoDniPosterior = fotoDniPosteriorUrl,
            Observacion = dto.Observacion,
            Estado = "PENDIENTE",           // Siempre PENDIENTE al crear
            EstadoAprobado = "PENDIENTE",   // Siempre PENDIENTE al crear
            FRegistro = DateTime.Now
        };

        await _unitOfWork.Repository<Negociacion>().AddAsync(negociacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        negociacion.Comisionista = comisionista;
        if (dto.IdProveedor.HasValue)
        {
            negociacion.Proveedor = await _unitOfWork.Repository<Persona>()
                .GetByIdAsync(dto.IdProveedor.Value, cancellationToken);
        }
        if (dto.IdProducto.HasValue)
        {
            negociacion.Producto = await _unitOfWork.Repository<Producto>()
                .GetByIdAsync(dto.IdProducto.Value, cancellationToken);
        }

        return _mapper.Map<NegociacionDto>(negociacion);
    }
}