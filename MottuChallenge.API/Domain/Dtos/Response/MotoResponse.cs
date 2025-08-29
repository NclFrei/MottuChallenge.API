using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Response;

public class MotoResponse
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public int AreaId { get; set; }
}
