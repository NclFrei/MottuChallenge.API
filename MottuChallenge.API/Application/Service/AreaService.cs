
using System.Text.Json;

using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Models;
using MottuChallenge.API.Domain.Validator;


namespace MottuChallenge.API.Application.Service;

public class AreaService
{
    private readonly IAreaRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<AreaRequest> _validator;

    public AreaService(IAreaRepository repository, IMapper mapper, IValidator<AreaRequest> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<AreaResponse> CreateAsync(AreaRequest request)
    {
        var result = await _validator.ValidateAsync(request);

        if (!result.IsValid)
        {
            var errors = result.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage} ")
                .ToList();

            throw new ValidationException(string.Join(Environment.NewLine, errors));
        }
        
        var area = _mapper.Map<Area>(request);
        var created = await _repository.CreateAsync(area);
        return _mapper.Map<AreaResponse>(created);
    }

    public async Task<PagedResponse<AreaResponse>> GetAllAsync(string? nome, int? patioId, int page = 1, int limit = 10)
    {
        if (page <= 0) page = 1;
        
        if (limit <= 0 || limit > 100) limit = 10;
        var query = _repository.Query();
        
        if (!string.IsNullOrEmpty(nome))
            query = query.Where(m => m.Nome == nome);
        
        if(patioId.HasValue)
            query = query.Where(m => m.PatioId == patioId);
        
        var total = await query.CountAsync();
        
        var areas = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
        
        var areasResponse = _mapper.Map<List<AreaResponse>>(areas);
        
        return new PagedResponse<AreaResponse>(
            areasResponse,
            page,
            limit,
            total,
            total > page * limit ? $"/api/area?page={page + 1}&limit={limit}" : null,
            page > 1 ? $"/api/area?page={page - 1}&limit={limit}" : null
        );
    }

    public async Task<AreaResponse?> GetByIdAsync(int id)
    {
        var area = await _repository.GetByIdAsync(id);
        return area == null ? null : _mapper.Map<AreaResponse>(area);
    }

    public async Task<AreaResponse?> UpdateAsync(int id, AtualizarAreaRequest request)
    {
        var existingArea = await _repository.GetByIdAsync(id);
        if (existingArea == null) return null;

        _mapper.Map(request, existingArea);
        
        var updated = await _repository.UpdateAsync(existingArea);
        return _mapper.Map<AreaResponse>(updated);
    }
    
    public async Task<AreaResponse?> ReplaceAsync(int id, AreaRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
    
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                .ToList();

            throw new ValidationException(string.Join(Environment.NewLine, errors));
        }

        var existingArea = await _repository.GetByIdAsync(id);
        if (existingArea == null) return null;
        
        _mapper.Map(request, existingArea);

        var updated = await _repository.UpdateAsync(existingArea);
        return _mapper.Map<AreaResponse>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
