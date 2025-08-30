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

        public async Task<Patio> UpdateAsync(Patio patio)
        {
            _context.Patios.Update(patio);
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