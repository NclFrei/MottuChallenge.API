using System.Text.Json;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Domain.Interfaces;

public interface IMotoRepository
{
    Task<Moto> CreateAsync(Moto moto);
    Task<List<Moto>> GetAllAsync();
    Task<Moto?> GetByIdAsync(int id);
    Task<Moto?> UpdateAsync(Moto moto);
    Task<bool> DeleteAsync(int id);
}