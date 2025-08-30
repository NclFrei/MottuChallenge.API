using AutoMapper;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Application.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Criação de usuário
        CreateMap<UserCreateRequest, User>();

        // Retorno de usuário
        CreateMap<User, UserResponse>();
        
        CreateMap<AtualizarUserRequest, User>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    !(srcMember is string str && string.IsNullOrWhiteSpace(str))
                ));
    }
    
}