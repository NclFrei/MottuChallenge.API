using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Request;

public class EnderecoRequest
{
    public string Rua { get; set; }
    
    public long? Numero { get; set; }
    
    public string Bairro { get; set; }
    
    public string Cidade { get; set; }
    
    public string Estado { get; set; }
    
    public string Cep { get; set; }

    public string Complemento { get; set; }
}
