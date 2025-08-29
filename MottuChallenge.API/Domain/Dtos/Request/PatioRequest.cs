using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Request;

public class PatioRequest
{
    [Required(ErrorMessage = "O nome do patio é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O endereço do patio é obrigatório.")]
    public EnderecoRequest Endereco { get; set; }

    [Required(ErrorMessage = "Nenhum usuario está sendo associado a esse patio")]
    public int UserId { get; set; }
}
