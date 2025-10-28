using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MotoController : ControllerBase
{
    private readonly MotoService _service;

    public MotoController(MotoService service)
    {
        _service = service;
    }

    [HttpPost]    
    [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MotoResponse>> Create([FromBody] MotoRequest request)
    {
        var response = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<MotoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<MotoResponse>>> GetAll([FromQuery] string? modelo,[FromQuery] int? areaId,[FromQuery] int page = 1,[FromQuery] int limit = 10)
    {
        var response = await _service.GetAllAsync(modelo, areaId, page, limit);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MotoResponse>> GetById(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response == null)
            return NotFound($"Moto com ID {id} não encontrada.");

        return Ok(response);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MotoResponse>> PatchMoto(int id, [FromBody] AtualizarMotoRequest request)
    {
        if (request == null)
            return BadRequest("Request inválido.");

        var updated = await _service.UpdateAsync(id, request);
        if (updated == null)
            return NotFound($"Moto com ID {id} não encontrada para atualização.");

        return Ok(updated);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MotoResponse>> PutMoto(int id, [FromBody] MotoRequest request)
    {
        if (request == null)
            return BadRequest("Request inválido.");

        var updated = await _service.ReplaceAsync(id, request);
        if (updated == null)
            return NotFound($"Moto com ID {id} não encontrada.");

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound($"Moto com ID {id} não encontrada para exclusão.");

        return NoContent();
    }
}
