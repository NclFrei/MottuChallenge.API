using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Infrastructure.Data;

namespace MottuChallenge.API.Infrastructure.Repository;

public class AreaRepository : IAreaRepository
{
    private readonly MottuChallengeContext _context;

    public AreaRepository(MottuChallengeContext context)
    {
        _context = context;
    }

    public async Task<Area> CreateAsync(Area area)
    {
        _context.Areas.Add(area);
        await _context.SaveChangesAsync();
        return area;
    }

    public async Task<List<Area>> GetAllAsync()
    {
        return await _context.Areas
            .Include(a => a.Motos)
            .ToListAsync();
    }

    public async Task<Area?> GetByIdAsync(int id)
    {
        return await _context.Areas
            .Include(a => a.Motos)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Area?> UpdateAsync(int id, JsonElement request)
    {
        var area = await _context.Areas
            .Include(a => a.Motos)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (area == null) return null;

        bool alterado = false;

        if (request.TryGetProperty("nome", out var nomeProp))
        {
            var novoNome = nomeProp.GetString();
            if (!string.IsNullOrWhiteSpace(novoNome) && novoNome != area.Nome)
            {
                area.Nome = novoNome;
                alterado = true;
            }
        }

        if (request.TryGetProperty("patioId", out var patioIdProp))
        {
            var novoPatioId = patioIdProp.GetInt32();
            if (novoPatioId != area.PatioId)
            {
                area.PatioId = novoPatioId;
                alterado = true;
            }
        }

        if (alterado)
            await _context.SaveChangesAsync();

        return area;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area == null) return false;

        _context.Areas.Remove(area);
        await _context.SaveChangesAsync();
        return true;
    }
}