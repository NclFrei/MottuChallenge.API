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
using System.Text.Json;
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

        var patio = PatioConverter.ParaPatio(patioRequest, endereco.Id);
        _context.Patios.Add(patio);
        await _context.SaveChangesAsync();

        endereco.IdPatio = patio.Id;
        _context.Enderecos.Update(endereco);
        await _context.SaveChangesAsync();

        var user = await _context.User.FindAsync(patio.UserId);

        return PatioConverter.ParaPatioResponse(patio, endereco);
    }

    public async Task<List<PatioResponse>> GetAllPatiosAsync()
    {
        var patios = await _context.Patios
        .Include(p => p.Endereco)
        .Include(p => p.User)
        .Include(p => p.Areas)
        .ToListAsync();

        return patios.Select(p =>
            PatioConverter.ParaPatioResponse(p, p.Endereco)
        ).ToList();
    }

    public async Task<PatioResponse?> GetPatioByIdAsync(Guid id)
    {
        var patio = await _context.Patios
         .Include(p => p.Endereco)
         .Include(p => p.User)
         .Include(p => p.Areas)
         .FirstOrDefaultAsync(p => p.Id == id);

        if (patio == null) return null;

        return PatioConverter.ParaPatioResponse(patio, patio.Endereco);
    }

    public async Task<PatioResponse?> UpdatePatioAsync(Guid id, JsonElement request)
    {
        var patio = await _context.Patios
            .Include(p => p.Endereco)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (patio == null)
            return null;

        bool alterado = false;

        if (request.TryGetProperty("nome", out var nomeProp))
        {
            var novoNome = nomeProp.GetString();
            if (!string.IsNullOrWhiteSpace(novoNome) && novoNome != patio.Nome)
            {
                patio.Nome = novoNome;
                alterado = true;
            }
        }

        if (request.TryGetProperty("userId", out var userIdProp))
        {
            var novoUserId = userIdProp.GetGuid();
            if (novoUserId != patio.UserId)
            {
                patio.UserId = novoUserId;
                alterado = true;
            }
        }

        if (request.TryGetProperty("endereco", out var enderecoProp) && patio.Endereco != null)
        {
            if (EnderecoConverter.AtualizarEndereco(patio.Endereco, enderecoProp))
                alterado = true;
        }

        if (alterado)
            await _context.SaveChangesAsync();

        var user = await _context.User.FindAsync(patio.UserId);
        return PatioConverter.ParaPatioResponse(patio, patio.Endereco);
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
