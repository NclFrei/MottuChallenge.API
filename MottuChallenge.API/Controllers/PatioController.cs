using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Erros;

namespace MottuChallenge.API.Controllers;

[Route("ChallengeMottu/[controller]")]
[ApiController]
[Authorize]
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
    [ProducesResponseType(typeof(IEnumerable<PatioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PatioResponse>>> GetPatios()
    {
        var patios = await _patioService.GetAllPatiosAsync();
        return Ok(patios);
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
