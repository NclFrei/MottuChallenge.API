using System.Text.Json;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Domain.Interfaces;

public interface IAreaRepository
{
    Task<Area> CreateAsync(Area area);
    Task<List<Area>> GetAllAsync();
    Task<Area?> GetByIdAsync(int id);
    Task<Area?> UpdateAsync(int id, JsonElement request);
    Task<bool> DeleteAsync(int id);
}