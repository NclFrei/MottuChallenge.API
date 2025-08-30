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

public class AreaService
    {
        private readonly MottuChallengeContext _context;
        private readonly IMapper _mapper;

        public AreaService(MottuChallengeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AreaResponse> CreateAsync(AreaRequest request)
        {
            var area = _mapper.Map<Area>(request);
            _context.Areas.Add(area);
            await _context.SaveChangesAsync();
            return _mapper.Map<AreaResponse>(area);
        }

        public async Task<List<AreaResponse>> GetAllAsync()
        {
            var areas = await _context.Areas
                .Include(a => a.Motos)
                .ToListAsync();

            return _mapper.Map<List<AreaResponse>>(areas);
        }

        public async Task<AreaResponse?> GetByIdAsync(int id)
        {
            var area = await _context.Areas
                .Include(a => a.Motos)
                .FirstOrDefaultAsync(a => a.Id == id);

            return area == null ? null : _mapper.Map<AreaResponse>(area);
        }

        public async Task<AreaResponse?> UpdateAsync(int id, JsonElement request)
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

            return _mapper.Map<AreaResponse>(area);
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
