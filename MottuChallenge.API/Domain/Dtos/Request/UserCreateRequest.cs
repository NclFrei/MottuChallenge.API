using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Request;

public class UserCreateRequest
{
    public string Nome { get; set; }

    public string Email { get; set; }
    
    public string Password { get; set; }

}
