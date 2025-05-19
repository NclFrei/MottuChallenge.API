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

public static class AreaConverter
{
    public static Area ParaArea(AreaRequest request)
    {
        return new Area
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            PatioId = request.PatioId
        };
    }

    public static AreaResponse ParaAreaResponse(Area area)
    {
        return new AreaResponse
        {
            Id = area.Id,
            Nome = area.Nome,
            PatioId = area.PatioId,
            Motos = area.Motos?.Select(MotoConverter.ParaMotoResponse).ToList() ?? new()
        };
    }

    public static void AtualizarArea(Area area, AreaRequest request)
    {
        area.Nome = request.Nome;
        area.PatioId = request.PatioId;
    }

    public static bool AtualizarArea(Area area, JsonElement request)
    {
        bool alterado = false;

        if (request.TryGetProperty("nome", out var nomeProp))
        {
            string novoNome = nomeProp.GetString();
            if (!string.IsNullOrWhiteSpace(novoNome) && novoNome != area.Nome)
            {
                area.Nome = novoNome;
                alterado = true;
            }
        }

        if (request.TryGetProperty("patioId", out var patioProp))
        {
            Guid novoPatioId = patioProp.GetGuid();
            if (novoPatioId != area.PatioId)
            {
                area.PatioId = novoPatioId;
                alterado = true;
            }
        }

        return alterado;
    }
}
