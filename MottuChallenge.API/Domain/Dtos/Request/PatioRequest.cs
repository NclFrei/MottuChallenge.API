using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Request;

public class PatioRequest
{
    public string Nome { get; set; }
    
    public EnderecoRequest Endereco { get; set; }
    
    public int? UserId { get; set; }
}
