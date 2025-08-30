using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Infrastructure.Data;

namespace MottuChallenge.API.Infrastructure.Repository;

public class PatioRepository : IPatioRepository
    {
        private readonly MottuChallengeContext _context;

        public PatioRepository(MottuChallengeContext context)
        {
            _context = context;
        }

        public async Task<Patio> CreateAsync(Patio patio, Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            patio.EnderecoId = endereco.Id;
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            endereco.IdPatio = patio.Id;
            _context.Enderecos.Update(endereco);
            await _context.SaveChangesAsync();

            return patio;
        }

        public async Task<List<Patio>> GetAllAsync()
        {
            return await _context.Patios
                .Include(p => p.Endereco)
                .Include(p => p.User)
                .Include(p => p.Areas)
                .ToListAsync();
        }

        public async Task<Patio?> GetByIdAsync(int id)
        {
            return await _context.Patios
                .Include(p => p.Endereco)
                .Include(p => p.User)
                .Include(p => p.Areas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patio?> UpdateAsync(int id, JsonElement request)
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
                if (enderecoUpdate != null)
                {
                    patio.Endereco.Rua = enderecoUpdate.Rua;
                    patio.Endereco.Numero = enderecoUpdate.Numero;
                    patio.Endereco.Bairro = enderecoUpdate.Bairro;
                    patio.Endereco.Cidade = enderecoUpdate.Cidade;
                    patio.Endereco.Estado = enderecoUpdate.Estado;
                    patio.Endereco.Cep = enderecoUpdate.Cep;
                    alterado = true;
                }
            }

            if (alterado)
                await _context.SaveChangesAsync();

            return patio;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null) return false;

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();
            return true;
        }
    }