using MottuChallenge.Domain.Dtos.Request;
using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            PatioId = area.PatioId
        };
    }

    public static void AtualizarArea(Area area, AreaRequest request)
    {
        area.Nome = request.Nome;
        area.PatioId = request.PatioId;
    }
}
