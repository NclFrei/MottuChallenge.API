using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Application.Converter;

public class EnderecoConverter
{
    public static Endereco paraEndereco(EnderecoRequest enderecoRequest)
    {
       return new Endereco
        {
            Rua = enderecoRequest.Rua,
            Numero = enderecoRequest.Numero,
            Bairro = enderecoRequest.Bairro,
            Cidade = enderecoRequest.Cidade,
            Estado = enderecoRequest.Estado,
            Cep = enderecoRequest.Cep,
            Complemento = enderecoRequest.Complemento
        };
        
    }

    public static EnderecoResponse ParaEnderecoResponse(Endereco endereco)
    {
        return new EnderecoResponse
        {
            Id = endereco.Id,
            Rua = endereco.Rua,
            Numero = endereco.Numero,
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            Complemento = endereco.Complemento,
            Estado = endereco.Estado,
            Cep = endereco.Cep
        };
    }
}
