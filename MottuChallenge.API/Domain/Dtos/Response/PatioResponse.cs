
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Response;

public class PatioResponse
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public EnderecoResponse Endereco { get; set; }
    public int UserId { get; set; }
    public List<AreaResponse> Areas { get; set; }
}
