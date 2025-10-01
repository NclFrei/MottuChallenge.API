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


public class PatioService
{
    private readonly IPatioRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<PatioRequest> _validator;

    public PatioService(IPatioRepository repository, IMapper mapper, IValidator<PatioRequest> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator; 
    }

    public async Task<PatioResponse> CreatePatioAsync(PatioRequest patioRequest)
    {
        var validationResult = await _validator.ValidateAsync(patioRequest);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                .ToList();

            throw new ValidationException(string.Join(Environment.NewLine, errors));
        }


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

    public async Task<PatioResponse?> GetPatioByUserIdAsync(int userid)
    {
        var patio = await _repository.GetByIdAsync(userid);
        return patio == null ? null : _mapper.Map<PatioResponse>(patio);
    }


    public async Task<PatioResponse?> GetPatioByIdAsync(int id)
    {
        var patio = await _repository.GetByIdAsync(id);
        return patio == null ? null : _mapper.Map<PatioResponse>(patio);
    }

    public async Task<PatioResponse?> UpdatePatioAsync(int id, AtualizarPatioRequest request)
    {
        var existingPatio = await _repository.GetByIdAsync(id);
        if (existingPatio == null) return null;
        
        _mapper.Map(request, existingPatio);
        
        if (request.Endereco != null)
        {
            if (existingPatio.Endereco == null)
            {
                existingPatio.Endereco = _mapper.Map<Endereco>(request.Endereco);
            }
            else
            {
                _mapper.Map(request.Endereco, existingPatio.Endereco);
            }
        }

        var updated = await _repository.UpdateAsync(existingPatio);
        return _mapper.Map<PatioResponse>(updated);
    }

    public async Task<bool> DeletePatioAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}

