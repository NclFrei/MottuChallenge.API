using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Domain.Models;

public class Area
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public Guid PatioId { get; set; }
    public Patio Patio { get; set; }
}
