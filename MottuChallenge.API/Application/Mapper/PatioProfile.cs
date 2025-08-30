using AutoMapper;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Application.Mapper;

public class PatioProfile : Profile
{
    public PatioProfile()
    {
        // Request -> Model
        CreateMap<EnderecoRequest, Endereco>();
        CreateMap<PatioRequest, Patio>()
            .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco));

        // Model -> Response
        CreateMap<Endereco, EnderecoResponse>();
    
        CreateMap<Patio, PatioResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)) // 👈 só pega o IdUser
            .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco))
            .ForMember(dest => dest.Areas, opt => opt.MapFrom(src => src.Areas));
        
        CreateMap<AtualizarPatioRequest, Patio>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    !(srcMember is string str && string.IsNullOrWhiteSpace(str))
                ));
    }
}