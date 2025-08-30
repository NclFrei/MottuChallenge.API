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

public class MotoService
    {
        private readonly MottuChallengeContext _context;
        private readonly IMapper _mapper;

        public MotoService(MottuChallengeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MotoResponse> CreateAsync(MotoRequest request)
        {
            var moto = _mapper.Map<Moto>(request);
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return _mapper.Map<MotoResponse>(moto);
        }

        public async Task<List<MotoResponse>> GetAllAsync()
        {
            var motos = await _context.Motos.ToListAsync();
            return _mapper.Map<List<MotoResponse>>(motos);
        }

        public async Task<MotoResponse?> GetByIdAsync(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            return moto == null ? null : _mapper.Map<MotoResponse>(moto);
        }

        public async Task<MotoResponse?> UpdateAsync(int id, JsonElement request)
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

            return _mapper.Map<MotoResponse>(moto);
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