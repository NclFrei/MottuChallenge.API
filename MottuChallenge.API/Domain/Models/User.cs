namespace MottuChallenge.API.Domain.Models;

public class User
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; } 

    public string Password { get; set; }

    public ICollection<Patio> Patios { get; set; }

    public void SetPassword(string password)
    {
        Password = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool CheckPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Password);
    }


}