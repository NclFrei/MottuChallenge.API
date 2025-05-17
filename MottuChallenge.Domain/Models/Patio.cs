using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Domain.Models;

public class Patio
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public Guid EnderecoId { get; set; }
    public Endereco Endereco { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }
}
