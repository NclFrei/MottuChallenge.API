using MottuChallenge.Domain.Dtos.Response;
using MottuChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Application.Converter;

public class UserConverter
{
    public static UserResponse ParaUserResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Nome = user.Nome,
            Email = user.Email
        };
    }
}
