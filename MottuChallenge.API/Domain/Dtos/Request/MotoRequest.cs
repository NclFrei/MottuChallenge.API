using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Request;

public class MotoRequest
{
    public string Placa { get; set; }
    
    public string Modelo { get; set; }
    
    public int? AreaId { get; set; }
}


