using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;
using Asp.Versioning;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Erros;

namespace MottuChallenge.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PatioController : ControllerBase
{
    private readonly PatioService _patioService;

    public PatioController(PatioService patioService)
    {
        _patioService = patioService;
    }


    [HttpPost]
    [ProducesResponseType(typeof(PatioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PatioResponse>> CreatePatio([FromBody] PatioRequest patioRequest)
    {
        var patioResponse = await _patioService.CreatePatioAsync(patioRequest);
        return CreatedAtAction(null, new { id = patioResponse.Id }, patioResponse);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<PatioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<PatioResponse>>> GetAll([FromQuery] int page = 1,[FromQuery] int limit = 10)
    {
        var response = await _patioService.GetAllPatiosAsync(page, limit);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IEnumerable<PatioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PatioResponse>> GetPatioById(int id)
    {
        var patio = await _patioService.GetPatioByIdAsync(id);
        if (patio == null)
            return NotFound($"Nenhum pátio encontrado com o ID {id}");


        return Ok(patio);
    }


    [HttpGet("Userid/{id}")]
    [ProducesResponseType(typeof(IEnumerable<PatioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PatioResponse>> GetPatioByUserId(int id)
    {
        var patio = await _patioService.GetPatioByUserIdAsync(id);
        if (patio == null)
            return NotFound($"Nenhum pátio encontrado para esse user ID {id}");


        return Ok(patio);
    }


    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(PatioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PatioResponse>> PatchPatio(int id, [FromBody] AtualizarPatioRequest request)
    {
        if (request == null)
            return BadRequest("Request inválido.");

        var updated = await _patioService.UpdatePatioAsync(id, request);
        if (updated == null)
            return NotFound($"Pátio com ID {id} não encontrado para atualização.");

        return Ok(updated);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PatioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PatioResponse>> PutPatio(int id, [FromBody] PatioRequest request)
    {
        if (request == null)
            return BadRequest("Request inválido.");

        var updated = await _patioService.ReplacePatioAsync(id, request);
        if (updated == null)
            return NotFound($"Pátio com ID {id} não encontrado.");

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePatio(int id)
    {
        var deleted = await _patioService.DeletePatioAsync(id);
        if (!deleted)
            return NotFound($"Não foi possível remover: pátio com ID {id} não encontrado.");

        return NoContent();
    }
}
