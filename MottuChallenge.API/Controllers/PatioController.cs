using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.Service;
using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Models;

namespace MottuChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatioController : ControllerBase
    {
        private readonly PatioService _patioService;

        public PatioController(PatioService patioService)
        {
            _patioService = patioService;
        }

        [HttpPost]
        public async Task<ActionResult<PatioResponse>> CreatePatio([FromBody] PatioRequest patioRequest)
        {
            var patioResponse = await _patioService.CreatePatioAsync(patioRequest);
            return CreatedAtAction(null, new { id = patioResponse.Id }, patioResponse);
        }
    }
}
