using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Infrastructure.Data;

namespace MottuChallenge.API.Application.Service;


    public class PatioService
    {
        private readonly MottuChallengeContext _context;
        private readonly IMapper _mapper;

        public PatioService(MottuChallengeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PatioResponse> CreatePatioAsync(PatioRequest patioRequest)
        {
            var endereco = _mapper.Map<Endereco>(patioRequest.Endereco);
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            var patio = _mapper.Map<Patio>(patioRequest);
            patio.EnderecoId = endereco.Id;

            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            endereco.IdPatio = patio.Id;
            _context.Enderecos.Update(endereco);
            await _context.SaveChangesAsync();

            return _mapper.Map<PatioResponse>(patio);
        }

        public async Task<List<PatioResponse>> GetAllPatiosAsync()
        {
            var patios = await _context.Patios
                .Include(p => p.Endereco)
                .Include(p => p.User)
                .Include(p => p.Areas)
                .ToListAsync();

            return _mapper.Map<List<PatioResponse>>(patios);
        }

        public async Task<PatioResponse?> GetPatioByIdAsync(int id)
        {
            var patio = await _context.Patios
                .Include(p => p.Endereco)
                .Include(p => p.User)
                .Include(p => p.Areas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patio == null) return null;

            return _mapper.Map<PatioResponse>(patio);
        }

        public async Task<PatioResponse?> UpdatePatioAsync(int id, JsonElement request)
        {
            var patio = await _context.Patios
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patio == null) return null;

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
                var novoUserId = userIdProp.GetInt16();
                if (novoUserId != patio.UserId)
                {
                    patio.UserId = novoUserId;
                    alterado = true;
                }
            }

            if (request.TryGetProperty("endereco", out var enderecoProp) && patio.Endereco != null)
            {
                
                var enderecoUpdate = JsonSerializer.Deserialize<EnderecoRequest>(enderecoProp.GetRawText());
                _mapper.Map(enderecoUpdate, patio.Endereco);
                alterado = true;
            }

            if (alterado)
                await _context.SaveChangesAsync();

            return _mapper.Map<PatioResponse>(patio);
        }

        public async Task<bool> DeletePatioAsync(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null) return false;

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();
            return true;
        }
    }

