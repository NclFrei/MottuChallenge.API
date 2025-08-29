namespace MottuChallenge.API.Domain.Models;

public class Area
{
    public int Id { get; set; }
    public string Nome { get; set; }

    public int PatioId { get; set; }
    public Patio Patio { get; set; }
    public ICollection<Moto> Motos { get; set; }
}