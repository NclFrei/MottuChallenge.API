using Microsoft.EntityFrameworkCore;
using MottuChallenge.Application.Converter;
using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Application.Service;

public class AreaService
{
    private readonly MottuChallengeContext _context;

    public AreaService(MottuChallengeContext context)
    {
        _context = context;
    }

    public async Task<AreaResponse> CreateAsync(AreaRequest request)
    {
        var area = AreaConverter.ParaArea(request);
        _context.Areas.Add(area);
        await _context.SaveChangesAsync();
        return AreaConverter.ParaAreaResponse(area);
    }

    public async Task<List<AreaResponse>> GetAllAsync()
    {
        var areas = await _context.Areas.ToListAsync();
        return areas.Select(AreaConverter.ParaAreaResponse).ToList();
    }

    public async Task<AreaResponse?> GetByIdAsync(Guid id)
    {
        var area = await _context.Areas.FindAsync(id);
        return area == null ? null : AreaConverter.ParaAreaResponse(area);
    }

    public async Task<AreaResponse?> UpdateAsync(Guid id, AreaRequest request)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area == null) return null;

        AreaConverter.AtualizarArea(area, request);
        await _context.SaveChangesAsync();

        return AreaConverter.ParaAreaResponse(area);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area == null) return false;

        _context.Areas.Remove(area);
        await _context.SaveChangesAsync();
        return true;
    }
}
