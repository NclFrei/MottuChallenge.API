using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.Service;
using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using System.Text.Json;

namespace MottuChallenge.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MotoController : ControllerBase
{
    private readonly MotoService _service;

    public MotoController(MotoService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<MotoResponse>> Create([FromBody] MotoRequest request)
    {
        var response = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MotoResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MotoResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MotoResponse>> GetById(Guid id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response == null)
            return NotFound($"Moto com ID {id} não encontrada.");

        return Ok(response);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(MotoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MotoResponse>> Patch(Guid id, [FromBody] JsonElement request)
    {
        var response = await _service.UpdateAsync(id, request);
        if (response == null)
            return NotFound($"Moto com ID {id} não encontrada.");

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound($"Moto com ID {id} não encontrada para exclusão.");

        return NoContent();
    }
}
