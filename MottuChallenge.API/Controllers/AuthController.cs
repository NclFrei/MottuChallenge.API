using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.Service;
using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Models;
using MottuChallenge.Infrastructure.Data;

namespace MottuChallenge.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly MottuChallengeContext _context;
    private readonly UserService _userService;

    public AuthController(MottuChallengeContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
    {
        var userResponse = await _userService.CreateUserAsync(userRequest);
        return Created($"/users/{userResponse.Id}", userResponse);
    }

}
