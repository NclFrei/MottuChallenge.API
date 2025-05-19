using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Domain.Dtos.Response;

public class MotoResponse
{
    public Guid Id { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public Guid AreaId { get; set; }
}
