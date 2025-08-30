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
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Infrastructure.Data;

namespace MottuChallenge.API.Application.Service;

public class MotoService
{
    private readonly IMotoRepository _repository;
    private readonly IMapper _mapper;

    public MotoService(IMotoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MotoResponse> CreateAsync(MotoRequest request)
    {
        var moto = _mapper.Map<Moto>(request);
        var created = await _repository.CreateAsync(moto);
        return _mapper.Map<MotoResponse>(created);
    }

    public async Task<List<MotoResponse>> GetAllAsync()
    {
        var motos = await _repository.GetAllAsync();
        return _mapper.Map<List<MotoResponse>>(motos);
    }

    public async Task<MotoResponse?> GetByIdAsync(int id)
    {
        var moto = await _repository.GetByIdAsync(id);
        return moto == null ? null : _mapper.Map<MotoResponse>(moto);
    }

    public async Task<MotoResponse?> UpdateAsync(int id, JsonElement request)
    {
        var moto = await _repository.UpdateAsync(id, request);
        return moto == null ? null : _mapper.Map<MotoResponse>(moto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}