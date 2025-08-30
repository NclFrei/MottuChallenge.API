using AutoMapper;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Application.Mapper;

public class AreaProfile : Profile
{
    public AreaProfile()
    {
        // Requests -> Models
        CreateMap<AreaRequest, Area>();

        // Models -> Responses
        CreateMap<Area, AreaResponse>()
            .ForMember(dest => dest.Motos, opt => opt.MapFrom(src => src.Motos));
    }
}