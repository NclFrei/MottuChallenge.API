using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
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
    private readonly IValidator<MotoRequest> _validator;

    public MotoService(IMotoRepository repository, IMapper mapper, IValidator<MotoRequest> validator )
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<MotoResponse> CreateAsync(MotoRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                .ToList();

            throw new ValidationException(string.Join(Environment.NewLine, errors));
        }
        
        
        var moto = _mapper.Map<Moto>(request);
        var created = await _repository.CreateAsync(moto);
        return _mapper.Map<MotoResponse>(created);
    }

    public async Task<PagedResponse<MotoResponse>> GetAllAsync(string? modelo, int? areaId, int page, int limit = 10)
    {
        if (page <= 0) page = 1;

        if (limit <= 0 || limit > 100) limit = 10;
        var query = _repository.Query();
        
        if (!string.IsNullOrEmpty(modelo))
            query = query.Where(m => m.Modelo == modelo);
        
        if(areaId.HasValue)
            query = query.Where(m => m.AreaId == areaId);
        
        var total = await query.CountAsync();
        
        var motos = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
        
        var motoResponses = _mapper.Map<List<MotoResponse>>(motos);
        
        return new PagedResponse<MotoResponse>(
            motoResponses,
            page,
            limit,
            total,
            total > page * limit ? $"/api/moto?page={page + 1}&limit={limit}" : null,
            page > 1 ? $"/api/moto?page={page - 1}&limit={limit}" : null
        );
    }

    public async Task<MotoResponse?> GetByIdAsync(int id)
    {
        var moto = await _repository.GetByIdAsync(id);
        return moto == null ? null : _mapper.Map<MotoResponse>(moto);
    }

    public async Task<MotoResponse?> UpdateAsync(int id, AtualizarMotoRequest request)
    {
        var existingMoto = await _repository.GetByIdAsync(id);
        if (existingMoto == null) return null;
        
        _mapper.Map(request, existingMoto);

        var updated = await _repository.UpdateAsync(existingMoto);
        return _mapper.Map<MotoResponse>(updated);
    }
    
    public async Task<MotoResponse?> ReplaceAsync(int id, MotoRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                .ToList();

            throw new ValidationException(string.Join(Environment.NewLine, errors));
        }

        var existingMoto = await _repository.GetByIdAsync(id);
        if (existingMoto == null) return null;

        // PUT sobrescreve TODOS os campos da entidade
        _mapper.Map(request, existingMoto);

        var updated = await _repository.UpdateAsync(existingMoto);
        return _mapper.Map<MotoResponse>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}