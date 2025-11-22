using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Usuarios.Commands.CreateUsuario;
using Miski.Application.Features.Usuarios.Commands.UpdateUsuario;
using Miski.Application.Features.Usuarios.Commands.DeleteUsuario;
using Miski.Application.Features.Usuarios.Commands.UpdatePassword;
using Miski.Application.Features.Usuarios.Queries.GetUsuarios;
using Miski.Application.Features.Usuarios.Queries.GetUsuarioById;
using Miski.Application.Features.Usuarios.Queries.GetPersonasSinUsuario;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Usuarios;
using Miski.Shared.DTOs.Personas;

namespace Miski.Api.Controllers.Usuarios;

[ApiController]
[Route("api/usuarios")]
[Tags("Usuarios")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los usuarios con filtros opcionales
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - estado: Filtrar por estado (ACTIVO/INACTIVO)
    /// - idPersona: Filtrar por persona asociada
    /// 
    /// Incluye información de la persona asociada y roles asignados.
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UsuarioDto>>>> GetUsuarios(
        [FromQuery] string? estado = null,
        [FromQuery] int? idPersona = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetUsuariosQuery(estado, idPersona);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<UsuarioDto>>.SuccessResult(
                result, 
                "Usuarios obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<UsuarioDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene un usuario por ID
    /// </summary>
    /// <remarks>
    /// Retorna la información completa del usuario incluyendo:
    /// - Datos básicos del usuario
    /// - Información de la persona asociada
    /// - Roles asignados
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> GetUsuarioById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetUsuarioByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<UsuarioDto>.SuccessResult(
                result,
                "Usuario obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<UsuarioDto>.ErrorResult(
                "Usuario no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UsuarioDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene usuarios activos
    /// </summary>
    /// <remarks>
    /// Retorna solo los usuarios con estado "ACTIVO".
    /// Útil para listados en formularios y selecciones.
    /// </remarks>
    [HttpGet("activos")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UsuarioDto>>>> GetUsuariosActivos(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetUsuariosQuery(Estado: "ACTIVO");
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<UsuarioDto>>.SuccessResult(
                result, 
                "Usuarios activos obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<UsuarioDto>>.ErrorResult(
                "Error al obtener usuarios activos", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene personas que no tienen usuario creado
    /// </summary>
    /// <remarks>
    /// Retorna la lista de personas que aún no tienen un usuario asociado en el sistema.
    /// Útil para crear nuevos usuarios seleccionando de personas existentes.
    /// 
    /// Parámetros opcionales:
    /// - estado: Filtrar por estado de la persona (ACTIVO/INACTIVO)
    /// - idUsuario: ID del usuario para incluir su persona en la lista (útil para edición)
    /// 
    /// Cuando se proporciona idUsuario, la persona asociada a ese usuario se incluye en la lista,
    /// permitiendo editar el usuario y mantener o cambiar la persona asignada.
    /// </remarks>
    [HttpGet("personas-disponibles")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PersonaDto>>>> GetPersonasSinUsuario(
        [FromQuery] string? estado = null,
        [FromQuery] int? idUsuario = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetPersonasSinUsuarioQuery(estado, idUsuario);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<PersonaDto>>.SuccessResult(
                result, 
                "Personas sin usuario obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<PersonaDto>>.ErrorResult(
                "Error al obtener personas sin usuario", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    /// <remarks>
    /// Campos requeridos:
    /// - Username: Nombre de usuario único (3-50 caracteres)
    /// - Password: Contraseña (mínimo 6 caracteres)
    /// 
    /// Campos opcionales:
    /// - IdPersona: ID de la persona asociada
    /// - Estado: ACTIVO (por defecto) o INACTIVO
    /// - RolesIds: Lista de IDs de roles a asignar
    /// 
    /// La contraseña se hashea automáticamente con SHA256.
    /// </remarks>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> CreateUsuario(
        [FromBody] CreateUsuarioDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateUsuarioCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetUsuarioById), 
                new { id = result.IdUsuario }, 
                ApiResponse<UsuarioDto>.SuccessResult(
                    result, 
                    "Usuario creado exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<UsuarioDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<UsuarioDto>.ErrorResult(
                "Entidad relacionada no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UsuarioDto>.ErrorResult(
                "Error interno del servidor", 
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    /// <remarks>
    /// Permite actualizar:
    /// - Username
    /// - IdPersona
    /// - Estado
    /// - RolesIds (se eliminan roles no incluidos y se agregan nuevos)
    /// 
    /// NO actualiza la contraseña (usar endpoint específico PUT /api/usuarios/{id}/password)
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> UpdateUsuario(
        int id,
        [FromBody] UpdateUsuarioDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdUsuario)
            {
                return BadRequest(ApiResponse<UsuarioDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateUsuarioCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<UsuarioDto>.SuccessResult(
                result,
                "Usuario actualizado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<UsuarioDto>.ErrorResult(
                "Usuario no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<UsuarioDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UsuarioDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Desactiva un usuario (eliminación lógica)
    /// </summary>
    /// <remarks>
    /// NO elimina físicamente el usuario, solo cambia su estado a "INACTIVO".
    /// Esto permite mantener la integridad referencial y el historial.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteUsuario(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteUsuarioCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Usuario desactivado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Usuario no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza la contraseña de un usuario
    /// </summary>
    /// <remarks>
    /// Endpoint específico para cambiar la contraseña.
    /// La nueva contraseña debe tener mínimo 6 caracteres y se hashea automáticamente con SHA256.
    /// 
    /// Campos requeridos:
    /// - IdUsuario: ID del usuario
    /// - NewPassword: Nueva contraseña
    /// </remarks>
    [HttpPut("{id}/password")]
    public async Task<ActionResult<ApiResponse>> UpdatePassword(
        int id,
        [FromBody] UpdatePasswordDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdUsuario)
            {
                return BadRequest(ApiResponse.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdatePasswordCommand(id, request);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Contraseña actualizada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Usuario no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }
}
