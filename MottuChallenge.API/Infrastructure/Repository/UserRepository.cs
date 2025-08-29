using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Infrastructure.Data;

namespace MottuChallenge.API.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly MottuChallengeContext _context;

    public UserRepository(MottuChallengeContext context)
    {
        _context = context;
    }
    

    public async Task<bool> VerificaEmailExisteAsync(string email)
    {
        return await _context.User.AnyAsync(u => u.Email == email);
    }
    
    public async Task<User?> BuscarPorEmailAsync(string email)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CriarAsync(User usuario)
    {
        _context.User.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }
    
}