
using System.Text.Json;

using AutoMapper;
using FluentValidation;
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

    public async Task<AreaResponse?> UpdateAsync(int id, AtualizarAreaRequest request)
    {
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
