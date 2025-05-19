using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

    public static bool AtualizarEndereco(Endereco endereco, JsonElement enderecoProp)
    {
        bool alterado = false;

        if (enderecoProp.TryGetProperty("rua", out var rua) && rua.GetString() != endereco.Rua)
        {
            endereco.Rua = rua.GetString();
            alterado = true;
        }

        if (enderecoProp.TryGetProperty("numero", out var numero) && numero.GetInt64() != endereco.Numero)
        {
            endereco.Numero = numero.GetInt64();
            alterado = true;
        }

        if (enderecoProp.TryGetProperty("bairro", out var bairro) && bairro.GetString() != endereco.Bairro)
        {
            endereco.Bairro = bairro.GetString();
            alterado = true;
        }

        if (enderecoProp.TryGetProperty("cidade", out var cidade) && cidade.GetString() != endereco.Cidade)
        {
            endereco.Cidade = cidade.GetString();
            alterado = true;
        }

        if (enderecoProp.TryGetProperty("estado", out var estado) && estado.GetString() != endereco.Estado)
        {
            endereco.Estado = estado.GetString();
            alterado = true;
        }

        if (enderecoProp.TryGetProperty("cep", out var cep) && cep.GetString() != endereco.Cep)
        {
            endereco.Cep = cep.GetString();
            alterado = true;
        }

        if (enderecoProp.TryGetProperty("complemento", out var complemento) && complemento.GetString() != endereco.Complemento)
        {
            endereco.Complemento = complemento.GetString();
            alterado = true;
        }

        return alterado;
    }
}
