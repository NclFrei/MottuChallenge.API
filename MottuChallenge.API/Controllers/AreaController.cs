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
public class AreaController : ControllerBase
{
    private readonly AreaService _service;

    public AreaController(AreaService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AreaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AreaResponse>> Create([FromBody] AreaRequest request)
    {
        var response = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<AreaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<AreaResponse>>> GetAll([FromQuery] string? nome,[FromQuery] int? patioId,[FromQuery] int page = 1,[FromQuery] int limit = 10)
    {
        var response = await _service.GetAllAsync(nome, patioId, page, limit);
        return Ok(response);
    }
    

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AreaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AreaResponse>> GetById(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response == null)
            return NotFound($"Área com ID {id} não encontrada.");

        return Ok(response);
    }
    
    
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(AreaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AreaResponse>> PatchArea(int id, [FromBody] AtualizarAreaRequest request)
    {
        if (request == null)
            return BadRequest("Request inválido.");

        var updated = await _service.UpdateAsync(id, request);
        if (updated == null)
            return NotFound($"Área com ID {id} não encontrada para atualização.");

        return Ok(updated);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AreaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AreaResponse>> PutArea(int id, [FromBody] AreaRequest request)
    {
        if (request == null)
            return BadRequest("Request inválido.");

        var updated = await _service.ReplaceAsync(id, request);
        if (updated == null)
            return NotFound($"Área com ID {id} não encontrada.");

        return Ok(updated);
    }
    
    

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound($"Área com ID {id} não encontrada para exclusão.");

        return NoContent();
    }
}
