namespace MottuChallenge.API.Domain.Dtos.Request;

public class AtualizarUserRequest
{
    public string? Nome { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
}