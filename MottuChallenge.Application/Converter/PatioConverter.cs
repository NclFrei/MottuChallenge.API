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
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            EnderecoId = enderecoId,
            UserId = request.UserId
        };
    }

    public static void AtualizarPatio(Patio patio, PatioRequest request)
    {
        patio.Nome = request.Nome;
        patio.UserId = request.UserId;

        if (patio.Endereco != null && request.Endereco != null)
        {
            patio.Endereco.Rua = request.Endereco.Rua;
            patio.Endereco.Numero = request.Endereco.Numero;
            patio.Endereco.Bairro = request.Endereco.Bairro;
            patio.Endereco.Cidade = request.Endereco.Cidade;
            patio.Endereco.Estado = request.Endereco.Estado;
            patio.Endereco.Complemento = request.Endereco.Complemento;
            patio.Endereco.Cep = request.Endereco.Cep;
        }
    }

    public static PatioResponse ParaPatioResponse(Patio patio, Endereco endereco, User user)
    {
        return new PatioResponse
        {
            Id = patio.Id,
            Nome = patio.Nome,
            Endereco = EnderecoConverter.ParaEnderecoResponse(endereco),
            Usuario = UserConverter.ParaUserResponse(user),
            Areas = patio.Areas?.Select(AreaConverter.ParaAreaResponse).ToList() ?? new()
        };
    }


}
