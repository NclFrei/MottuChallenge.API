using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Application.Converter;

public class PatioConverter
{
    public static Patio ParaPatio(PatioRequest request, Guid enderecoId)
    {
        return new Patio
        {
            Nome = request.Nome,
            EnderecoId = enderecoId,
            UserId = request.UserId
        };
    }

    public static List<PatioResponse> ParaListaResponse(IEnumerable<Patio> patios)
    {
        return patios.Select(p => ParaPatioResponse(p, p.Endereco)).ToList();
    }

    public static void AtualizarPatio(Patio patio, PatioRequest request)
    {
        patio.Nome = request.Nome;
        patio.UserId = request.UserId;
    }

    public static PatioResponse ParaPatioResponse(Patio patio, Endereco endereco)
    {
        return new PatioResponse
        {
            Id = patio.Id,
            Nome = patio.Nome,
            Endereco = EnderecoConverter.ParaEnderecoResponse(endereco),
            UserId = patio.UserId,
            Areas = patio.Areas?.Select(AreaConverter.ParaAreaResponse).ToList() ?? new()
        };
    }


}
