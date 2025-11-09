using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MottuChallenge.API.Application.Service;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class MlController : ControllerBase
{
    private readonly MlService _mlService;
    private readonly IMotoRepository _motoRepository;

    public MlController(MlService mlService, IMotoRepository motoRepository)
    {
        _mlService = mlService;
        _motoRepository = motoRepository;
    }

    [HttpGet("cluster")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Cluster([FromQuery] int clusters = 3)
    {
        var motos = await _motoRepository.GetAllAsync();
        var result = _mlService.ClusterMotos(motos, clusters)
            .Select(r => new { MotoId = r.MotoId, Cluster = r.Cluster });

        return Ok(result);
    }
}
