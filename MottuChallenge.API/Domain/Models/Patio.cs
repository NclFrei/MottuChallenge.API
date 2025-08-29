namespace MottuChallenge.API.Domain.Models;

public class Patio
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int EnderecoId { get; set; }
    public Endereco Endereco { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<Area> Areas { get; set; }
}