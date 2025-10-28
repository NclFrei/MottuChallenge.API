using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> BuscarPorIdAsync(int id);
    Task<bool> VerificaEmailExisteAsync(string email);
    Task<User?> BuscarPorEmailAsync(string email);
    Task<User> CriarAsync(User usuario);
    Task<bool> DeleteAsync(User usuario);
    Task<User> AtualizarAsync(User usuario);
    IQueryable<User> Query();
}
