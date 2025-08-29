using Microsoft.EntityFrameworkCore;
using MottuChallenge.Application.Converter;
using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Infrastructure.Data;

namespace MottuChallenge.API.Application.Service;

public class MotoService
{
    private readonly MottuChallengeContext _context;

    public MotoService(MottuChallengeContext context)
    {
        _context = context;
    }

    public async Task<MotoResponse> CreateAsync(MotoRequest request)
    {
        var moto = MotoConverter.ParaMoto(request);
        _context.Motos.Add(moto);
        await _context.SaveChangesAsync();
        return MotoConverter.ParaMotoResponse(moto);
    }

    public async Task<List<MotoResponse>> GetAllAsync()
    {
        var motos = await _context.Motos.ToListAsync();
        return motos.Select(MotoConverter.ParaMotoResponse).ToList();
    }

    public async Task<MotoResponse?> GetByIdAsync(int id)
    {
        var moto = await _context.Motos.FindAsync(id);
        return moto == null ? null : MotoConverter.ParaMotoResponse(moto);
    }

    public async Task<MotoResponse?> UpdateAsync(int id, JsonElement request)
    {
        var moto = await _context.Motos.FindAsync(id);
        if (moto == null) return null;

        if (MotoConverter.AtualizarMoto(moto, request))
            await _context.SaveChangesAsync();

        return MotoConverter.ParaMotoResponse(moto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var moto = await _context.Motos.FindAsync(id);
        if (moto == null) return false;

        _context.Motos.Remove(moto);
        await _context.SaveChangesAsync();
        return true;
    }
}
