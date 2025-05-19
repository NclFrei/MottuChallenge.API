using MottuChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Domain.Dtos.Response;

public class PatioResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public EnderecoResponse Endereco { get; set; }
    public Guid UserId { get; set; }
    public List<AreaResponse> Areas { get; set; }
}
