using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Domain.Dtos.Request;

public class LoginRequest
{
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    public string Email { get; set; }
    public string Password { get; set; }
}
