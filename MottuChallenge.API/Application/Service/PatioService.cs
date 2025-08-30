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


public class PatioService
{
    private readonly IPatioRepository _repository;
    private readonly IMapper _mapper;

    public PatioService(IPatioRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PatioResponse> CreatePatioAsync(PatioRequest patioRequest)
    {
        var patio = _mapper.Map<Patio>(patioRequest);
        var endereco = _mapper.Map<Endereco>(patioRequest.Endereco);

        var created = await _repository.CreateAsync(patio, endereco);
        return _mapper.Map<PatioResponse>(created);
    }

    public async Task<List<PatioResponse>> GetAllPatiosAsync()
    {
        var patios = await _repository.GetAllAsync();
        return _mapper.Map<List<PatioResponse>>(patios);
    }

    public async Task<PatioResponse?> GetPatioByIdAsync(int id)
    {
        var patio = await _repository.GetByIdAsync(id);
        return patio == null ? null : _mapper.Map<PatioResponse>(patio);
    }

    public async Task<PatioResponse?> UpdatePatioAsync(int id, JsonElement request)
    {
        var patio = await _repository.UpdateAsync(id, request);
        return patio == null ? null : _mapper.Map<PatioResponse>(patio);
    }

    public async Task<bool> DeletePatioAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}

