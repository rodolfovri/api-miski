using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.FAQ.CategoriaFAQ.Commands.CreateCategoria;
using Miski.Application.Features.FAQ.CategoriaFAQ.Commands.UpdateCategoria;
using Miski.Application.Features.FAQ.CategoriaFAQ.Commands.DeleteCategoria;
using Miski.Application.Features.FAQ.CategoriaFAQ.Queries.GetCategorias;
using Miski.Application.Features.FAQ.CategoriaFAQ.Queries.GetCategoriaById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Api.Controllers.FAQ;

[ApiController]
[Route("api/faq/categorias")]
[Tags("FAQ")]
[Authorize]
public class CategoriaFAQController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriaFAQController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las categorías de FAQ
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - estado: Filtrar por estado (ACTIVO, INACTIVO)
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaFAQDto>>>> GetCategorias(
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCategoriasQuery(estado);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<CategoriaFAQDto>>.SuccessResult(
                result,
                "Categorías obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<CategoriaFAQDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una categoría de FAQ por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaFAQDto>>> GetCategoriaById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetCategoriaByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<CategoriaFAQDto>.SuccessResult(
                result,
                "Categoría obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CategoriaFAQDto>.ErrorResult(
                "Categoría no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CategoriaFAQDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva categoría de FAQ
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoriaFAQDto>>> CreateCategoria(
        [FromBody] CreateCategoriaFAQDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateCategoriaCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetCategoriaById),
                new { id = result.IdCategoriaFAQ },
                ApiResponse<CategoriaFAQDto>.SuccessResult(
                    result,
                    "Categoría creada exitosamente"
                )
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CategoriaFAQDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una categoría de FAQ
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaFAQDto>>> UpdateCategoria(
        int id,
        [FromBody] UpdateCategoriaFAQDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdCategoriaFAQ)
            {
                return BadRequest(ApiResponse<CategoriaFAQDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateCategoriaCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<CategoriaFAQDto>.SuccessResult(
                result,
                "Categoría actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<CategoriaFAQDto>.ErrorResult(
                "Categoría no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<CategoriaFAQDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina una categoría de FAQ (cambio de estado a INACTIVO)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteCategoria(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteCategoriaCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Categoría eliminada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Categoría no encontrada",
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
