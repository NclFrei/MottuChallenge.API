using Microsoft.EntityFrameworkCore;
using MottuChallenge.Application.Converter;
using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Models;
using MottuChallenge.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Application.Service;

public class PatioService
{
    public MottuChallengeContext _context;

    public PatioService(MottuChallengeContext context)
    {
        _context = context;
    }

    public async Task<PatioResponse> CreatePatioAsync(PatioRequest patioRequest)
    {
        var endereco = EnderecoConverter.paraEndereco(patioRequest.Endereco);
        _context.Enderecos.Add(endereco);
        await _context.SaveChangesAsync();

        var patio = PatioConverter.paraPatio(patioRequest, endereco.Id);
        _context.Patios.Add(patio);
        await _context.SaveChangesAsync();

        endereco.IdPatio = patio.Id;
        _context.Enderecos.Update(endereco);
        await _context.SaveChangesAsync();

        var user = await _context.User.FindAsync(patio.UserId);

        return PatioConverter.ParaPatioResponse(patio, endereco, user);
    }

    public async Task<List<PatioResponse>> GetAllPatiosAsync()
    {
        var patios = await _context.Patios.Include(p => p.Endereco).Include(p => p.User).ToListAsync();
        return patios.Select(p => PatioConverter.ParaPatioResponse(p, p.Endereco, p.User)).ToList();
    }

    public async Task<PatioResponse?> GetPatioByIdAsync(Guid id)
    {
        var patio = await _context.Patios
            .Include(p => p.Endereco)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (patio == null) return null;

        return PatioConverter.ParaPatioResponse(patio, patio.Endereco, patio.User);
    }

    public async Task<PatioResponse?> UpdatePatioAsync(Guid id, PatioRequest request)
    {
        var patio = await _context.Patios
       .Include(p => p.Endereco)
       .FirstOrDefaultAsync(p => p.Id == id);

        if (patio == null) 
            return null;

        PatioConverter.AtualizarPatio(patio, request);

        await _context.SaveChangesAsync();

        var user = await _context.User.FindAsync(patio.UserId);
        return PatioConverter.ParaPatioResponse(patio, patio.Endereco, user);
    }

    public async Task<bool> DeletePatioAsync(Guid id)
    {
        var patio = await _context.Patios.FindAsync(id);
        if (patio == null) return false;

        _context.Patios.Remove(patio);
        await _context.SaveChangesAsync();
        return true;
    }

}
