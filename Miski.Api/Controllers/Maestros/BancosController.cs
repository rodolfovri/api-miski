using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.Maestros.Banco.Commands.CreateBanco;
using Miski.Application.Features.Maestros.Banco.Commands.UpdateBanco;
using Miski.Application.Features.Maestros.Banco.Commands.DeleteBanco;
using Miski.Application.Features.Maestros.Banco.Queries.GetBancos;
using Miski.Application.Features.Maestros.Banco.Queries.GetBancoById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Api.Controllers.Maestros;

[ApiController]
[Route("api/maestros/bancos")]
[Tags("Maestros")]
[Authorize]
public class BancosController : ControllerBase
{
    private readonly IMediator _mediator;

    public BancosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los bancos
    /// </summary>
    /// <param name="estado">Filtrar por estado (opcional)</param>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BancoDto>>>> GetBancos(
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetBancosQuery(estado);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<IEnumerable<BancoDto>>.SuccessResult(
                result,
                "Bancos obtenidos exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<BancoDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene un banco por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BancoDto>>> GetBancoById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetBancoByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<BancoDto>.SuccessResult(
                result,
                "Banco obtenido exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<BancoDto>.ErrorResult(
                "Banco no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<BancoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea un nuevo banco
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<BancoDto>>> CreateBanco(
        [FromBody] CreateBancoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateBancoCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetBancoById),
                new { id = result.IdBanco },
                ApiResponse<BancoDto>.SuccessResult(
                    result,
                    "Banco creado exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<BancoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<BancoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza un banco
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<BancoDto>>> UpdateBanco(
        int id,
        [FromBody] UpdateBancoDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdBanco)
            {
                return BadRequest(ApiResponse<BancoDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateBancoCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<BancoDto>.SuccessResult(
                result,
                "Banco actualizado exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<BancoDto>.ErrorResult(
                "Banco no encontrado",
                ex.Message
            ));
        }
        catch (Shared.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<BancoDto>.ValidationErrorResult(ex.Errors));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<BancoDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina (inactiva) un banco
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteBanco(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteBancoCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Banco eliminado exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Banco no encontrado",
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
}
