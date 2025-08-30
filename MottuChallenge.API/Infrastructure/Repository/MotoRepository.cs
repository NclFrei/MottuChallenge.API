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

        public async Task<Moto?> UpdateAsync(int id, JsonElement request)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return null;

            bool alterado = false;

            if (request.TryGetProperty("placa", out var placaProp))
            {
                var novaPlaca = placaProp.GetString();
                if (!string.IsNullOrWhiteSpace(novaPlaca) && novaPlaca != moto.Placa)
                {
                    moto.Placa = novaPlaca;
                    alterado = true;
                }
            }

            if (request.TryGetProperty("modelo", out var modeloProp))
            {
                var novoModelo = modeloProp.GetString();
                if (!string.IsNullOrWhiteSpace(novoModelo) && novoModelo != moto.Modelo)
                {
                    moto.Modelo = novoModelo;
                    alterado = true;
                }
            }

            if (request.TryGetProperty("areaId", out var areaIdProp))
            {
                var novaAreaId = areaIdProp.GetInt32();
                if (novaAreaId != moto.AreaId)
                {
                    moto.AreaId = novaAreaId;
                    alterado = true;
                }
            }

            if (alterado)
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
    }