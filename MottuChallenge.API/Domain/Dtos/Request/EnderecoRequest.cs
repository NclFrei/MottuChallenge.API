using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.API.Domain.Dtos.Request;

public class EnderecoRequest
{
    [Required(ErrorMessage = "O nome da rua é obrigatório.")]
    public string Rua { get; set; }

    [Required(ErrorMessage = "O numero do endereço é obrigatório.")]
    public long Numero { get; set; }

    [Required(ErrorMessage = "O bairro é obrigatório.")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "A Cidade é obrigatório.")]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "O Estado é obrigatório.")]
    public string Estado { get; set; }

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "CEP inválido. Use o formato 12345-678 ou 12345678.")]
    public string Cep { get; set; }

    public string Complemento { get; set; }
}
