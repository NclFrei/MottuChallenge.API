using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MottuChallenge.Application.Service;
using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Models;
using MottuChallenge.Infrastructure.Data;

namespace MottuChallenge.API.Controllers;


[Route("ChallengeMottu/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly MottuChallengeContext _context;
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public AuthController(MottuChallengeContext context, UserService userService, TokenService tokenService)
    {
        _context = context;
        _userService = userService;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
    {
        var userResponse = await _userService.CreateUserAsync(userRequest);
        return Created($"/users/{userResponse.Id}", userResponse);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)] // retorna token
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.User.SingleOrDefault(u => u.Email == request.Email);
        if (user == null || !user.CheckPassword(request.Password))
            return Unauthorized("Usuário ou senha inválidos");

        var token = _tokenService.GenerateToken(user);
        return Ok(new { token });
    }

}
