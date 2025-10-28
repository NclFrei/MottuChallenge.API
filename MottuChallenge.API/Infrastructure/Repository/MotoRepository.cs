using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Infrastructure.Data;

namespace MottuChallenge.API.Infrastructure.Repository;

public class MotoRepository : IMotoRepository
    {
        private readonly MottuChallengeContext _context;

        public MotoRepository(MottuChallengeContext context)
        {
            _context = context;
        }

        public async Task<Moto> CreateAsync(Moto moto)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return moto;
        }

        public async Task<List<Moto>> GetAllAsync()
        {
            return await _context.Motos.ToListAsync();
        }

        public async Task<Moto?> GetByIdAsync(int id)
        {
            return await _context.Motos.FindAsync(id);
        }
        
        
        public async Task<Moto?> UpdateAsync(Moto moto)
        {
            _context.Motos.Update(moto);
            await _context.SaveChangesAsync();
            return moto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return false;

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return true;
        }
        public IQueryable<Moto> Query()
        {
            return _context.Motos.AsQueryable();
        }
    }