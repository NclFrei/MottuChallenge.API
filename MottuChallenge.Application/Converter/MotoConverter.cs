using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Enums;
using MottuChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MottuChallenge.Application.Converter;

public class MotoConverter
{
    public static Moto ParaMoto(MotoRequest request)
    {
        return new Moto
        {
            Id = Guid.NewGuid(),
            Placa = request.Placa,
            Modelo = request.Modelo, // string
            AreaId = request.AreaId
        };
    }

    public static MotoResponse ParaMotoResponse(Moto moto)
    {
        return new MotoResponse
        {
            Id = moto.Id,
            Placa = moto.Placa,
            Modelo = moto.Modelo,
            AreaId = moto.AreaId
        };
    }

    public static bool AtualizarMoto(Moto moto, JsonElement request)
    {
        bool alterado = false;

        if (request.TryGetProperty("placa", out var placaProp))
        {
            var novaPlaca = placaProp.GetString();
            if (!string.IsNullOrWhiteSpace(novaPlaca) && novaPlaca != moto.Placa)
            {
                moto.Placa = novaPlaca;
                alterado = true;
            }
        }

        if (request.TryGetProperty("modelo", out var modeloProp))
        {
            var novoModelo = modeloProp.GetString();
            if (!string.IsNullOrWhiteSpace(novoModelo) && novoModelo != moto.Modelo)
            {
                moto.Modelo = novoModelo;
                alterado = true;
            }
        }

        if (request.TryGetProperty("areaId", out var areaProp))
        {
            var novaArea = areaProp.GetGuid();
            if (novaArea != moto.AreaId)
            {
                moto.AreaId = novaArea;
                alterado = true;
            }
        }

        return alterado;
    }
}
