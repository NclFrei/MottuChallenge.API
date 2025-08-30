namespace MottuChallenge.API.Domain.Dtos.Request;

public class AtualizarPatioRequest
{
    public string? Nome { get; set; } 

    public EnderecoRequest? Endereco { get; set; } 

    public int? UserId { get; set; }
}