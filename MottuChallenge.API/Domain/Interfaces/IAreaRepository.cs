using System.Text.Json;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Domain.Interfaces;

public interface IAreaRepository
{
    Task<Area> CreateAsync(Area area);
    Task<List<Area>> GetAllAsync();
    Task<Area?> GetByIdAsync(int id);
    Task<Area?> UpdateAsync(Area area);
    Task<bool> DeleteAsync(int id);
    IQueryable<Area> Query();
}