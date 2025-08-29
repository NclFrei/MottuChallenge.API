using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Models;
using MottuChallenge.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Infrastructure.Data;

namespace MottuChallenge.API.Application.Service;

public class UserService
{
    public MottuChallengeContext _context;

    public UserService(MottuChallengeContext context)
    {
        _context = context;
    }

    public async Task<UserResponse> CreateUserAsync(UserCreateRequest userCreateRequest)
    {
        if (await VerificaEmailExisteAsync(userCreateRequest.Email))
            throw new InvalidOperationException("Email já cadastrado.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Nome = userCreateRequest.Nome,
            Email = userCreateRequest.Email 
        };

        user.SetPassword(userCreateRequest.Password);

        _context.User.Add(user);
        await _context.SaveChangesAsync();

        return new UserResponse
        {
            Id = user.Id,
            Nome = user.Nome,
            Email = user.Email
        };
    }

    public async Task<bool> VerificaEmailExisteAsync(string email)
    {
        return await _context.User.CountAsync(u => u.Email == email) > 0;
    }
}
