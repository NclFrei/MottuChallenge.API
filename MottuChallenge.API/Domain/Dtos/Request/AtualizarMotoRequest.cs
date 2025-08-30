namespace MottuChallenge.API.Domain.Dtos.Request;

public class AtualizarMotoRequest
{
    public string? Placa { get; set; }
    public string? Modelo { get; set; }
    public int? AreaId { get; set; }
}