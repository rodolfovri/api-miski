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
    private readonly IConfiguracionService _configuracionService;

    public CreateNegociacionHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IConfiguracionService configuracionService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuracionService = configuracionService;
    }

    public async Task<NegociacionDto> Handle(CreateNegociacionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Negociacion;

        // Validar que el comisionista existe
        var comisionista = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(dto.IdComisionista, cancellationToken);
        
        if (comisionista == null)
            throw new NotFoundException("Usuario", dto.IdComisionista);

        // Validar que la variedad de producto existe si se proporciona
        if (dto.IdVariedadProducto.HasValue)
        {
            var variedadProducto = await _unitOfWork.Repository<VariedadProducto>()
                .GetByIdAsync(dto.IdVariedadProducto.Value, cancellationToken);
            
            if (variedadProducto == null)
                throw new NotFoundException("VariedadProducto", dto.IdVariedadProducto.Value);
        }

        // Obtener PESO_POR_SACO desde la configuración global
        decimal pesoPorSaco;
        try
        {
            pesoPorSaco = await _configuracionService.ObtenerDecimalAsync("PESO_POR_SACO", cancellationToken);
        }
        catch (NotFoundException)
        {
            // Si no existe la configuración, usar valor por defecto de 50 kg
            pesoPorSaco = 50m;
        }

        // Calcular PesoTotal: SacosTotales * PesoPorSaco
        decimal? pesoTotal = null;
        if (dto.SacosTotales.HasValue)
        {
            pesoTotal = dto.SacosTotales.Value * pesoPorSaco;
        }

        // Calcular MontoTotalPago: SacosTotales * PrecioUnitario * PesoPorSaco
        decimal? montoTotalPago = null;
        if (dto.SacosTotales.HasValue && dto.PrecioUnitario.HasValue)
        {
            montoTotalPago = dto.SacosTotales.Value * dto.PrecioUnitario.Value * pesoPorSaco;
        }

        // Crear la negociación - PRIMERA ETAPA
        var negociacion = new Negociacion
        {
            IdComisionista = dto.IdComisionista,
            IdVariedadProducto = dto.IdVariedadProducto,
            SacosTotales = dto.SacosTotales,
            TipoCalidad = dto.TipoCalidad,
            PrecioUnitario = dto.PrecioUnitario,
            PesoPorSaco = pesoPorSaco, // Asignar peso por saco obtenido desde configuración
            PesoTotal = pesoTotal, // Asignar peso total calculado
            MontoTotalPago = montoTotalPago, // Asignar monto total calculado
            Estado = "EN PROCESO",  // Estado inicial automático
            EstadoAprobacionIngeniero = "PENDIENTE",
            FRegistro = DateTime.UtcNow,
            // Otros campos se llenarán en etapas posteriores
            NroDocumentoProveedor = string.Empty,
            FotoDniFrontal = string.Empty,
            FotoDniPosterior = string.Empty,
            PrimeraEvidenciaFoto = string.Empty,
            SegundaEvidenciaFoto = string.Empty,
            TerceraEvidenciaFoto = string.Empty,
            EvidenciaVideo = string.Empty
        };

        await _unitOfWork.Repository<Negociacion>().AddAsync(negociacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        negociacion.Comisionista = comisionista;

        if (dto.IdVariedadProducto.HasValue)
        {
            negociacion.VariedadProducto = await _unitOfWork.Repository<VariedadProducto>()
                .GetByIdAsync(dto.IdVariedadProducto.Value, cancellationToken);
            
            // ? CARGAR EL PRODUCTO DENTRO DE VARIEDAD PRODUCTO
            if (negociacion.VariedadProducto != null && negociacion.VariedadProducto.IdProducto > 0)
            {
                negociacion.VariedadProducto.Producto = await _unitOfWork.Repository<Producto>()
                    .GetByIdAsync(negociacion.VariedadProducto.IdProducto, cancellationToken);
            }
        }

        return _mapper.Map<NegociacionDto>(negociacion);
    }
}