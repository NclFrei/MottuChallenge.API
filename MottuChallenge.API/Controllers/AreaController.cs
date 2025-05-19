using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.Service;
using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using System.Text.Json;

namespace MottuChallenge.API.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    [ProducesResponseType(typeof(IEnumerable<AreaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AreaResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AreaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AreaResponse>> GetById(Guid id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response == null)
            return NotFound($"Área com ID {id} não encontrada.");

        return Ok(response);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(AreaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AreaResponse>> PatchArea(Guid id, [FromBody] JsonElement request)
    {
        var updated = await _service.UpdateAsync(id, request);
        if (updated == null)
            return NotFound($"Área com ID {id} não encontrada para atualização.");

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound($"Área com ID {id} não encontrada para exclusão.");

        return NoContent();
    }
}
