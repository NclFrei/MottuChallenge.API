using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Request;

public class UserRequest
{
    [Required(ErrorMessage = "Nome não pode ser vazio.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Email não pode ser vazio.")]
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha não pode ser vazio.")]
    public string Password { get; set; }

}
