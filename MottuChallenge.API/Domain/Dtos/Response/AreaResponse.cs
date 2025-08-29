using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Response;

public class AreaResponse
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int PatioId { get; set; }
    public List<MotoResponse> Motos { get; set; }
}
