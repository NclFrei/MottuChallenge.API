using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Request;

public class MotoRequest
{
    [Required(ErrorMessage = "A placa é obrigatória.")]
    [RegularExpression(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$", ErrorMessage = "Placa inválida.")]
    public string Placa { get; set; }

    [Required(ErrorMessage = "O modelo da moto é obrigatório.")]
    public string Modelo { get; set; }

    [Required(ErrorMessage = "O ID da área é obrigatório.")]
    public int AreaId { get; set; }
}
