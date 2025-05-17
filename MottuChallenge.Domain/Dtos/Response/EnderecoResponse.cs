using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Domain.Dtos.Response;

public class EnderecoResponse
{
    public Guid Id { get; set; }
    public string Rua { get; set; }
    public long Numero { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Cep { get; set; }
    public string Complemento { get; set; }
}
