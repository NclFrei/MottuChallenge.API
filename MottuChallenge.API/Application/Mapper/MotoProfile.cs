using AutoMapper;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Application.Mapper;

public class MotoProfile : Profile
{
    public MotoProfile()
    {
        // Requests -> Models
        CreateMap<MotoRequest, Moto>();

        // Models -> Responses
        CreateMap<Moto, MotoResponse>();
    }
}