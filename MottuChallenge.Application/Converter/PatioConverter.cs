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
    public static Patio paraPatio(PatioRequest patioRequest, Guid enderecoId)
    {
        return new Patio
        {
            Nome = patioRequest.Nome,
            EnderecoId = enderecoId,
            UserId = patioRequest.UserId // novo vínculo com usuário
        };
    }

    public static PatioResponse ParaPatioResponse(Patio patio, Endereco endereco, User user)
    {
        return new PatioResponse
        {
            Id = patio.Id,
            Nome = patio.Nome,
            Endereco = EnderecoConverter.ParaEnderecoResponse(endereco),
            Usuario = UserConverter.ParaUserResponse(user)
        };
    }
}
