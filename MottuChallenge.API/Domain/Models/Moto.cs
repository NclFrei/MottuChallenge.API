namespace MottuChallenge.API.Domain.Models;

public class Moto
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public int AreaId { get; set; }
    public Area Area { get; set; }
}