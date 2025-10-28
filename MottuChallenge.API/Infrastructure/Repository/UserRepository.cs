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
    
    public async Task<User?> BuscarPorIdAsync(int id)
    {
        return await _context.User.FindAsync(id);
    }


    public async Task<bool> VerificaEmailExisteAsync(string email)
    {
        return await _context.User.CountAsync(u => u.Email == email) > 0;
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
    
    public async Task<bool> DeleteAsync(User usuario)
    {
        _context.User.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<User> AtualizarAsync(User usuario)
    {
        _context.User.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }
    
    public IQueryable<User> Query()
    {
        return _context.User.AsQueryable();
    }
    
}