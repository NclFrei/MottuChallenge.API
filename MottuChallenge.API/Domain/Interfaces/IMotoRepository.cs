using System.Text.Json;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Domain.Interfaces;

public interface IMotoRepository
{
    Task<Moto> CreateAsync(Moto moto);
    Task<List<Moto>> GetAllAsync();
    Task<Moto?> GetByIdAsync(int id);
    Task<Moto?> UpdateAsync(int id, JsonElement request);
    Task<bool> DeleteAsync(int id);
}