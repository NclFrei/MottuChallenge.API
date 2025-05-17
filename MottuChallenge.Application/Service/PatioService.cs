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

}
