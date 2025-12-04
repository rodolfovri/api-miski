using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;
using Miski.Application.Services;

namespace Miski.Application.Features.Compras.Negociaciones.Commands.CompletarNegociacion;

public class CompletarNegociacionHandler : IRequestHandler<CompletarNegociacionCommand, NegociacionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;

    public CompletarNegociacionHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    public async Task<NegociacionDto> Handle(CompletarNegociacionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Completar;

        var negociacion = await _unitOfWork.Repository<Negociacion>()
            .GetByIdAsync(dto.IdNegociacion, cancellationToken);

        if (negociacion == null)
            throw new NotFoundException("Negociacion", dto.IdNegociacion);

        // VALIDACIÓN: Debe estar APROBADO por el ingeniero
        if (negociacion.EstadoAprobacionIngeniero != "APROBADO")
        {
            throw new ValidationException("Solo se pueden completar negociaciones aprobadas por el ingeniero");
        }

        // VALIDACIÓN: El estado debe ser APROBADO o EN REVISION
        if (negociacion.Estado != "APROBADO" && negociacion.Estado != "EN REVISION")
        {
            throw new ValidationException("Solo se pueden completar negociaciones en estado APROBADO o EN REVISION");
        }

        // VALIDACIÓN: Si está EN REVISION, solo se puede actualizar si NO ha sido aprobada por la contadora
        if (negociacion.Estado == "EN REVISION" && negociacion.EstadoAprobacionContadora == "APROBADO")
        {
            throw new ValidationException("No se pueden actualizar las evidencias de una negociación ya aprobada por la contadora");
        }

        // Si es la primera vez (estado APROBADO), validar que todas las fotos sean obligatorias
        if (negociacion.Estado == "APROBADO")
        {
            if (dto.FotoDniFrontal == null || dto.FotoDniPosterior == null ||
                dto.PrimeraEvidenciaFoto == null || dto.SegundaEvidenciaFoto == null ||
                dto.TerceraEvidenciaFoto == null || dto.EvidenciaVideo == null)
            {
                throw new ValidationException("Todas las fotos (DNI frontal, DNI posterior, 3 evidencias) y el video son obligatorios en la primera completación");
            }
        }

        // Validar que el tipo de documento existe si se proporciona
        if (dto.IdTipoDocumento.HasValue)
        {
            var tipoDocumento = await _unitOfWork.Repository<TipoDocumento>()
                .GetByIdAsync(dto.IdTipoDocumento.Value, cancellationToken);

            if (tipoDocumento == null)
                throw new NotFoundException("TipoDocumento", dto.IdTipoDocumento.Value);
        }

        // Validar que el banco existe si se proporciona
        if (dto.IdBanco.HasValue)
        {
            var banco = await _unitOfWork.Repository<Banco>()
                .GetByIdAsync(dto.IdBanco.Value, cancellationToken);

            if (banco == null)
                throw new NotFoundException("Banco", dto.IdBanco.Value);
        }

        // MANEJO DE PERSONA PROVEEDOR
        int? idPersonaProveedor = null;

        if (dto.IdPersona.HasValue)
        {
            var persona = await _unitOfWork.Repository<Persona>()
                .GetByIdAsync(dto.IdPersona.Value, cancellationToken);

            if (persona == null)
                throw new NotFoundException("Persona", dto.IdPersona.Value);

            bool personaActualizada = false;

            // VALIDACIÓN: Si cambia el documento, verificar que no exista en otra persona
            if (!string.IsNullOrEmpty(dto.NroDocumentoProveedor) &&
                persona.NumeroDocumento != dto.NroDocumentoProveedor)
            {
                var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
                var personaConMismoDocumento = personas.FirstOrDefault(p =>
                    p.NumeroDocumento == dto.NroDocumentoProveedor &&
                    p.IdPersona != dto.IdPersona.Value
                );

                if (personaConMismoDocumento != null)
                {
                    throw new ValidationException(
                        $"El número de documento {dto.NroDocumentoProveedor} ya está registrado para otra persona. " +
                        "Si deseas usar esa persona, debes buscarla y seleccionarla nuevamente."
                    );
                }

                persona.NumeroDocumento = dto.NroDocumentoProveedor;
                personaActualizada = true;
            }

            if (dto.IdTipoDocumento.HasValue &&
                persona.IdTipoDocumento != dto.IdTipoDocumento.Value)
            {
                persona.IdTipoDocumento = dto.IdTipoDocumento.Value;
                personaActualizada = true;
            }

            if (personaActualizada)
            {
                await _unitOfWork.Repository<Persona>().UpdateAsync(persona, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            idPersonaProveedor = dto.IdPersona.Value;
        }
        else if (!string.IsNullOrEmpty(dto.NroDocumentoProveedor))
        {
            // Si no se envía IdPersona pero sí NroDocumentoProveedor
            // Buscar si ya existe una persona con ese número de documento
            var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
            var personaExistente = personas.FirstOrDefault(p => p.NumeroDocumento == dto.NroDocumentoProveedor);

            if (personaExistente != null)
            {
                // Si existe, usar su IdPersona
                idPersonaProveedor = personaExistente.IdPersona;
            }
            else
            {
                // Si no existe, crear una nueva persona con datos pendientes
                // Validar que se haya proporcionado el tipo de documento
                if (!dto.IdTipoDocumento.HasValue)
                {
                    throw new ValidationException("El tipo de documento es obligatorio al crear un nuevo proveedor");
                }

                var nuevaPersona = new Persona
                {
                    IdTipoDocumento = dto.IdTipoDocumento.Value,
                    NumeroDocumento = dto.NroDocumentoProveedor,
                    Nombres = "PENDIENTE",
                    Apellidos = "PENDIENTE",
                    Estado = "ACTIVO",
                    FRegistro = DateTime.UtcNow
                };

                var personaCreada = await _unitOfWork.Repository<Persona>()
                    .AddAsync(nuevaPersona, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                idPersonaProveedor = personaCreada.IdPersona;
            }
        }

        // Actualizar solo las evidencias que se envíen (las que sean nuevas)
        // DNI Frontal
        if (dto.FotoDniFrontal != null)
        {
            // Si ya existía una foto anterior y se envía una nueva, eliminar la antigua
            if (!string.IsNullOrEmpty(negociacion.FotoDniFrontal))
            {
                await _fileStorageService.DeleteFileAsync(negociacion.FotoDniFrontal, cancellationToken);
            }

            var fotoDniFrontalUrl = await _fileStorageService.SaveFileAsync(
                dto.FotoDniFrontal, "negociaciones/dni", cancellationToken);
            negociacion.FotoDniFrontal = fotoDniFrontalUrl;
        }

        // DNI Posterior
        if (dto.FotoDniPosterior != null)
        {
            if (!string.IsNullOrEmpty(negociacion.FotoDniPosterior))
            {
                await _fileStorageService.DeleteFileAsync(negociacion.FotoDniPosterior, cancellationToken);
            }

            var fotoDniPosteriorUrl = await _fileStorageService.SaveFileAsync(
                dto.FotoDniPosterior, "negociaciones/dni", cancellationToken);
            negociacion.FotoDniPosterior = fotoDniPosteriorUrl;
        }

        // Primera Evidencia
        if (dto.PrimeraEvidenciaFoto != null)
        {
            if (!string.IsNullOrEmpty(negociacion.PrimeraEvidenciaFoto))
            {
                await _fileStorageService.DeleteFileAsync(negociacion.PrimeraEvidenciaFoto, cancellationToken);
            }

            var primeraEvidenciaUrl = await _fileStorageService.SaveFileAsync(
                dto.PrimeraEvidenciaFoto, "negociaciones/evidencias", cancellationToken);
            negociacion.PrimeraEvidenciaFoto = primeraEvidenciaUrl;
        }

        // Segunda Evidencia
        if (dto.SegundaEvidenciaFoto != null)
        {
            if (!string.IsNullOrEmpty(negociacion.SegundaEvidenciaFoto))
            {
                await _fileStorageService.DeleteFileAsync(negociacion.SegundaEvidenciaFoto, cancellationToken);
            }

            var segundaEvidenciaUrl = await _fileStorageService.SaveFileAsync(
                dto.SegundaEvidenciaFoto, "negociaciones/evidencias", cancellationToken);
            negociacion.SegundaEvidenciaFoto = segundaEvidenciaUrl;
        }

        // Tercera Evidencia
        if (dto.TerceraEvidenciaFoto != null)
        {
            if (!string.IsNullOrEmpty(negociacion.TerceraEvidenciaFoto))
            {
                await _fileStorageService.DeleteFileAsync(negociacion.TerceraEvidenciaFoto, cancellationToken);
            }

            var terceraEvidenciaUrl = await _fileStorageService.SaveFileAsync(
                dto.TerceraEvidenciaFoto, "negociaciones/evidencias", cancellationToken);
            negociacion.TerceraEvidenciaFoto = terceraEvidenciaUrl;
        }

        // Video
        if (dto.EvidenciaVideo != null)
        {
            if (!string.IsNullOrEmpty(negociacion.EvidenciaVideo))
            {
                await _fileStorageService.DeleteFileAsync(negociacion.EvidenciaVideo, cancellationToken);
            }

            var evidenciaVideoUrl = await _fileStorageService.SaveFileAsync(
                dto.EvidenciaVideo, "negociaciones/videos", cancellationToken);
            negociacion.EvidenciaVideo = evidenciaVideoUrl;
        }

        // Actualizar información del proveedor y banco
        negociacion.IdPersonaProveedor = idPersonaProveedor;
        negociacion.IdTipoDocumento = dto.IdTipoDocumento;
        negociacion.IdBanco = dto.IdBanco;
        negociacion.NroDocumentoProveedor = dto.NroDocumentoProveedor;
        negociacion.NroCuentaBancaria = dto.NroCuentaBancaria;

        // Cambiar estado a EN REVISION (o mantenerlo si ya estaba)
        negociacion.Estado = "EN REVISION";
        negociacion.EstadoAprobacionContadora = "PENDIENTE";

        await _unitOfWork.Repository<Negociacion>().UpdateAsync(negociacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relaciones para el DTO
        negociacion.Comisionista = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(negociacion.IdComisionista, cancellationToken) ?? new Usuario();

        // Cargar PersonaProveedor si existe
        if (negociacion.IdPersonaProveedor.HasValue)
        {
            negociacion.PersonaProveedor = await _unitOfWork.Repository<Persona>()
                .GetByIdAsync(negociacion.IdPersonaProveedor.Value, cancellationToken);
        }

        // Buscar proveedor por documento (para mantener compatibilidad con el campo antiguo)
        if (!string.IsNullOrEmpty(negociacion.NroDocumentoProveedor))
        {
            var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
            negociacion.Proveedor = personas.FirstOrDefault(p => p.NumeroDocumento == negociacion.NroDocumentoProveedor);
        }

        if (negociacion.IdVariedadProducto.HasValue)
        {
            negociacion.VariedadProducto = await _unitOfWork.Repository<VariedadProducto>()
                .GetByIdAsync(negociacion.IdVariedadProducto.Value, cancellationToken);

            // ? CARGAR EL PRODUCTO DENTRO DE VARIEDAD PRODUCTO
            if (negociacion.VariedadProducto != null && negociacion.VariedadProducto.IdProducto > 0)
            {
                negociacion.VariedadProducto.Producto = await _unitOfWork.Repository<Producto>()
                    .GetByIdAsync(negociacion.VariedadProducto.IdProducto, cancellationToken);
            }
        }

        if (negociacion.IdTipoDocumento.HasValue)
        {
            negociacion.TipoDocumento = await _unitOfWork.Repository<TipoDocumento>()
                .GetByIdAsync(negociacion.IdTipoDocumento.Value, cancellationToken);
        }

        if (negociacion.IdBanco.HasValue)
        {
            negociacion.Banco = await _unitOfWork.Repository<Banco>()
                .GetByIdAsync(negociacion.IdBanco.Value, cancellationToken);
        }

        if (negociacion.AprobadaPorIngeniero.HasValue)
        {
            negociacion.AprobadaPorUsuarioIngeniero = await _unitOfWork.Repository<Usuario>()
                .GetByIdAsync(negociacion.AprobadaPorIngeniero.Value, cancellationToken);
        }

        return _mapper.Map<NegociacionDto>(negociacion);
    }
}