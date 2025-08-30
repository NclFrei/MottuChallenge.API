using System.Text.Json;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Domain.Interfaces;

public interface IPatioRepository
{
    Task<Patio> CreateAsync(Patio patio, Endereco endereco);
    Task<List<Patio>> GetAllAsync();
    Task<Patio?> GetByIdAsync(int id);
    Task<Patio?> UpdateAsync(int id, JsonElement request);
    Task<bool> DeleteAsync(int id);
}