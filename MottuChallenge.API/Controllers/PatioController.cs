using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.Service;
using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Models;

namespace MottuChallenge.API.Controllers;

// [Authorize]
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
    public async Task<ActionResult<PatioResponse>> CreatePatio([FromBody] PatioRequest patioRequest)
    {
        var patioResponse = await _patioService.CreatePatioAsync(patioRequest);
        return CreatedAtAction(null, new { id = patioResponse.Id }, patioResponse);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatioResponse>>> GetPatios()
    {
        var patios = await _patioService.GetAllPatiosAsync();
        return Ok(patios);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatioResponse>> GetPatioById(Guid id)
    {
        var patio = await _patioService.GetPatioByIdAsync(id);
        if (patio == null)
            return NotFound($"Nenhum pátio encontrado com o ID {id}");


        return Ok(patio);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<PatioResponse>> UpdatePatio(Guid id, [FromBody] PatioRequest request)
    {
        var updated = await _patioService.UpdatePatioAsync(id, request);
        if (updated == null)
            return NotFound($"Pátio com ID {id} não encontrado para atualização.");

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatio(Guid id)
    {
        var deleted = await _patioService.DeletePatioAsync(id);
        if (!deleted)
            return NotFound($"Não foi possível remover: pátio com ID {id} não encontrado.");

        return NoContent();
    }
}
