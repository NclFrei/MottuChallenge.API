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

public class AreaService
{
    private readonly IAreaRepository _repository;
    private readonly IMapper _mapper;

    public AreaService(IAreaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AreaResponse> CreateAsync(AreaRequest request)
    {
        var area = _mapper.Map<Area>(request);
        var created = await _repository.CreateAsync(area);
        return _mapper.Map<AreaResponse>(created);
    }

    public async Task<List<AreaResponse>> GetAllAsync()
    {
        var areas = await _repository.GetAllAsync();
        return _mapper.Map<List<AreaResponse>>(areas);
    }

    public async Task<AreaResponse?> GetByIdAsync(int id)
    {
        var area = await _repository.GetByIdAsync(id);
        return area == null ? null : _mapper.Map<AreaResponse>(area);
    }

    public async Task<AreaResponse?> UpdateAsync(int id, JsonElement request)
    {
        var area = await _repository.UpdateAsync(id, request);
        return area == null ? null : _mapper.Map<AreaResponse>(area);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
