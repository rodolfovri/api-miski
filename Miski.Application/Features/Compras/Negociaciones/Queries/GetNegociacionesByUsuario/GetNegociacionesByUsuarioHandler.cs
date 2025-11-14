using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociacionesByUsuario;

public class GetNegociacionesByUsuarioHandler : IRequestHandler<GetNegociacionesByUsuarioQuery, List<NegociacionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetNegociacionesByUsuarioHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<NegociacionDto>> Handle(GetNegociacionesByUsuarioQuery request, CancellationToken cancellationToken)
    {
        // Validar que el usuario existe
        var usuario = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(request.IdUsuario, cancellationToken);

        if (usuario == null)
            throw new NotFoundException("Usuario", request.IdUsuario);

        // Obtener todas las negociaciones
        var negociaciones = await _unitOfWork.Repository<Negociacion>().GetAllAsync(cancellationToken);
        
        // Filtrar por IdComisionista (Usuario que creó la negociación)
        negociaciones = negociaciones.Where(n => n.IdComisionista == request.IdUsuario).ToList();

        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var variedadesProducto = await _unitOfWork.Repository<VariedadProducto>().GetAllAsync(cancellationToken);
        var productos = await _unitOfWork.Repository<Producto>().GetAllAsync(cancellationToken);
        var usuarios = await _unitOfWork.Repository<Usuario>().GetAllAsync(cancellationToken);

        // Cargar relaciones para cada negociación
        foreach (var negociacion in negociaciones)
        {
            // Buscar proveedor por documento si existe
            if (!string.IsNullOrEmpty(negociacion.NroDocumentoProveedor))
            {
                negociacion.Proveedor = personas.FirstOrDefault(p => p.NumeroDocumento == negociacion.NroDocumentoProveedor);
            }

            // Cargar comisionista (Usuario)
            negociacion.Comisionista = usuarios.FirstOrDefault(u => u.IdUsuario == negociacion.IdComisionista) ?? new Usuario();

            // Cargar variedad de producto
            if (negociacion.IdVariedadProducto.HasValue)
            {
                negociacion.VariedadProducto = variedadesProducto.FirstOrDefault(v => v.IdVariedadProducto == negociacion.IdVariedadProducto.Value);
                
                // ? CARGAR EL PRODUCTO DENTRO DE VARIEDAD PRODUCTO
                if (negociacion.VariedadProducto != null && negociacion.VariedadProducto.IdProducto > 0)
                {
                    negociacion.VariedadProducto.Producto = productos.FirstOrDefault(p => p.IdProducto == negociacion.VariedadProducto.IdProducto);
                }
            }

            // Cargar tipo de documento
            if (negociacion.IdTipoDocumento.HasValue)
            {
                var tiposDocumento = await _unitOfWork.Repository<TipoDocumento>().GetAllAsync(cancellationToken);
                negociacion.TipoDocumento = tiposDocumento.FirstOrDefault(t => t.IdTipoDocumento == negociacion.IdTipoDocumento.Value);
            }

            // Cargar banco
            if (negociacion.IdBanco.HasValue)
            {
                var bancos = await _unitOfWork.Repository<Banco>().GetAllAsync(cancellationToken);
                negociacion.Banco = bancos.FirstOrDefault(b => b.IdBanco == negociacion.IdBanco.Value);
            }

            // Cargar usuario aprobador ingeniero
            if (negociacion.AprobadaPorIngeniero.HasValue)
            {
                negociacion.AprobadaPorUsuarioIngeniero = usuarios.FirstOrDefault(u => u.IdUsuario == negociacion.AprobadaPorIngeniero.Value);
            }

            // Cargar usuario aprobador contadora
            if (negociacion.AprobadaPorContadora.HasValue)
            {
                negociacion.AprobadaPorUsuarioContadora = usuarios.FirstOrDefault(u => u.IdUsuario == negociacion.AprobadaPorContadora.Value);
            }

            // Cargar usuario rechazador ingeniero
            if (negociacion.RechazadoPorIngeniero.HasValue)
            {
                negociacion.RechazadoPorUsuarioIngeniero = usuarios.FirstOrDefault(u => u.IdUsuario == negociacion.RechazadoPorIngeniero.Value);
            }

            // Cargar usuario rechazador contadora
            if (negociacion.RechazadoPorContadora.HasValue)
            {
                negociacion.RechazadoPorUsuarioContadora = usuarios.FirstOrDefault(u => u.IdUsuario == negociacion.RechazadoPorContadora.Value);
            }
        }

        return negociaciones.Select(n => _mapper.Map<NegociacionDto>(n)).ToList();
    }
}
