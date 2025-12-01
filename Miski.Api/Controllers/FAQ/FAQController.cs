using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miski.Application.Features.FAQ.FAQs.Commands.CreateFAQ;
using Miski.Application.Features.FAQ.FAQs.Commands.UpdateFAQ;
using Miski.Application.Features.FAQ.FAQs.Commands.DeleteFAQ;
using Miski.Application.Features.FAQ.FAQs.Queries.GetFAQs;
using Miski.Application.Features.FAQ.FAQs.Queries.GetFAQById;
using Miski.Shared.DTOs.Base;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Api.Controllers.FAQ;

[ApiController]
[Route("api/faq")]
[Tags("FAQ")]
[Authorize]
public class FAQController : ControllerBase
{
    private readonly IMediator _mediator;

    public FAQController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las preguntas frecuentes (FAQ)
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - idCategoriaFAQ: Filtrar por categoría específica
    /// - estado: Filtrar por estado (ACTIVO, INACTIVO)
    /// 
    /// Las FAQs se devuelven ordenadas por el campo Orden.
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<FAQDto>>>> GetFAQs(
        [FromQuery] int? idCategoriaFAQ = null,
        [FromQuery] string? estado = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetFAQsQuery(idCategoriaFAQ, estado);
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(ApiResponse<IEnumerable<FAQDto>>.SuccessResult(
                result,
                "Preguntas frecuentes obtenidas exitosamente"
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<FAQDto>>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Obtiene una pregunta frecuente por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<FAQDto>>> GetFAQById(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetFAQByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(ApiResponse<FAQDto>.SuccessResult(
                result,
                "Pregunta frecuente obtenida exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<FAQDto>.ErrorResult(
                "Pregunta frecuente no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<FAQDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Crea una nueva pregunta frecuente
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<FAQDto>>> CreateFAQ(
        [FromBody] CreateFAQDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new CreateFAQCommand(request);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetFAQById),
                new { id = result.IdFAQ },
                ApiResponse<FAQDto>.SuccessResult(
                    result,
                    "Pregunta frecuente creada exitosamente"
                )
            );
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<FAQDto>.ErrorResult(
                "Categoría no encontrada",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<FAQDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Actualiza una pregunta frecuente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<FAQDto>>> UpdateFAQ(
        int id,
        [FromBody] UpdateFAQDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != request.IdFAQ)
            {
                return BadRequest(ApiResponse<FAQDto>.ErrorResult(
                    "ID inválido",
                    "El ID de la URL no coincide con el ID del cuerpo de la petición"
                ));
            }

            var command = new UpdateFAQCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<FAQDto>.SuccessResult(
                result,
                "Pregunta frecuente actualizada exitosamente"
            ));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<FAQDto>.ErrorResult(
                "Recurso no encontrado",
                ex.Message
            ));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<FAQDto>.ErrorResult(
                "Error interno del servidor",
                ex.Message
            ));
        }
    }

    /// <summary>
    /// Elimina una pregunta frecuente (cambio de estado a INACTIVO)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteFAQ(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteFAQCommand(id);
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.SuccessResult("Pregunta frecuente eliminada exitosamente"));
        }
        catch (Shared.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse.ErrorResult(
                "Pregunta frecuente no encontrada",
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
