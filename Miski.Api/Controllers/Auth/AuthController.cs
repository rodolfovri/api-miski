using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Miski.Application.Features.Auth.Commands.Login;
using Miski.Application.Features.Auth.Commands.Register;
using Miski.Application.Features.Auth.Commands.ChangePassword;
using Miski.Application.Features.Auth.Queries.GetUserProfile;
using Miski.Shared.DTOs.Auth;
using Miski.Shared.DTOs.Base;

namespace Miski.Api.Controllers.Auth;

[ApiController]
[Route("api/auth")]
[Tags("Auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Iniciar sesión
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(
        [FromBody] LoginDto loginDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new LoginCommand(loginDto);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<AuthResponseDto>.SuccessResult(
                result,
                "Inicio de sesión exitoso"
            ));
        }
        catch (Shared.Exceptions.NotFoundException)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.ErrorResult(
                "Credenciales inválidas",
                "Usuario no encontrado"
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<AuthResponseDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Registrar nuevo usuario
    /// </summary>
    [HttpPost("register")]
    //[Authorize(Roles = "Administrador")] // Solo administradores pueden crear usuarios
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register(
        [FromBody] RegisterDto registerDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new RegisterCommand(registerDto);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetProfile),
                new { },
                ApiResponse<AuthResponseDto>.SuccessResult(
                    result,
                    "Usuario registrado exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.ErrorResult(
                "Datos no encontrados",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.ValidationErrorResult(ex));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<AuthResponseDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtener perfil del usuario autenticado
    /// </summary>
    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> GetProfile(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(ApiResponse<AuthResponseDto>.ErrorResult(
                    "Token inválido",
                    "No se pudo obtener el ID del usuario"
                ));
            }

            var query = new GetUserProfileQuery(userId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<AuthResponseDto>.SuccessResult(
                result,
                "Perfil obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException)
        {
            return NotFound(ApiResponse<AuthResponseDto>.ErrorResult(
                "Usuario no encontrado",
                "El usuario no existe o ha sido eliminado"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<AuthResponseDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Cambiar contraseña del usuario autenticado
    /// </summary>
    [HttpPut("change-password")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<bool>>> ChangePassword(
        [FromBody] ChangePasswordDto changePasswordDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(ApiResponse<bool>.ErrorResult(
                    "Token inválido",
                    "No se pudo obtener el ID del usuario"
                ));
            }

            var command = new ChangePasswordCommand(userId, changePasswordDto);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<bool>.SuccessResult(
                result,
                "Contraseña cambiada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException)
        {
            return NotFound(ApiResponse<bool>.ErrorResult(
                "Usuario no encontrado",
                "El usuario no existe"
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<bool>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Cerrar sesión
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse>> Logout()
    {
        // En una implementación más robusta, aquí podrías:
        // 1. Invalidar el token en una lista negra
        // 2. Registrar el evento de logout
        // 3. Limpiar sesiones activas
        
        await Task.CompletedTask; // Simular operación async
        
        return Ok(ApiResponse.SuccessResult("Sesión cerrada exitosamente"));
    }

    /// <summary>
    /// Validar token (útil para el frontend)
    /// </summary>
    [HttpGet("validate-token")]
    [Authorize]
    public ActionResult<ApiResponse<object>> ValidateToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var usernameClaim = User.FindFirst(ClaimTypes.Name)?.Value;
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        var tokenInfo = new
        {
            IsValid = true,
            UserId = userIdClaim,
            Username = usernameClaim,
            Role = roleClaim,
            ExpiresAt = User.FindFirst("exp")?.Value
        };

        return Ok(ApiResponse<object>.SuccessResult(
            tokenInfo,
            "Token válido"
        ));
    }
}