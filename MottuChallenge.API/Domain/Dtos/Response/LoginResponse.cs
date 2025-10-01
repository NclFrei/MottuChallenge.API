namespace MottuChallenge.API.Domain.Dtos.Response;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}