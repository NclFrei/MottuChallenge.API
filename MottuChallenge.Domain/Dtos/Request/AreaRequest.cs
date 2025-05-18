using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Domain.Dtos.Request;

public class AreaRequest
{
    public string Nome { get; set; }
    public Guid PatioId { get; set; }
}
