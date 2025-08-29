using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;

namespace MottuChallenge.API.Controllers;

[Route("ChallengeMottu/[controller]")]
[ApiController]
public class PatioController : ControllerBase
{
    private readonly PatioService _patioService;

    public PatioController(PatioService patioService)
    {
        _patioService = patioService;
    }


    [HttpPost("RegisterPatio")]
    [ProducesResponseType(typeof(PatioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PatioResponse>> CreatePatio([FromBody] PatioRequest patioRequest)
    {
        var patioResponse = await _patioService.CreatePatioAsync(patioRequest);
        return CreatedAtAction(null, new { id = patioResponse.Id }, patioResponse);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PatioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PatioResponse>>> GetPatios()
    {
        var patios = await _patioService.GetAllPatiosAsync();
        return Ok(patios);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PatioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatioResponse>> GetPatioById(int id)
    {
        var patio = await _patioService.GetPatioByIdAsync(id);
        if (patio == null)
            return NotFound($"Nenhum pátio encontrado com o ID {id}");


        return Ok(patio);
    }


    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(PatioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PatioResponse>> PatchPatio(int id, [FromBody] JsonElement request)
    {
        var updated = await _patioService.UpdatePatioAsync(id, request);
        if (updated == null)
            return NotFound($"Pátio com ID {id} não encontrado para atualização.");

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePatio(int id)
    {
        var deleted = await _patioService.DeletePatioAsync(id);
        if (!deleted)
            return NotFound($"Não foi possível remover: pátio com ID {id} não encontrado.");

        return NoContent();
    }
}
