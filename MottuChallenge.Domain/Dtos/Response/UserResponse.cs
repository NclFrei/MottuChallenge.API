using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Domain.Dtos.Response;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } 
    public string Email { get; set; } 
}
