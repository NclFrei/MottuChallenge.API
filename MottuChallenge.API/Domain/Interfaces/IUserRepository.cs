using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> VerificaEmailExisteAsync(string email);
    Task<User?> BuscarPorEmailAsync(string email);
    Task<User> CriarAsync(User usuario);
}