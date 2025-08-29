using AutoMapper;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Infrastructure.Data;

namespace MottuChallenge.API.Application.Service;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<UserResponse?> GetUserByIdAsync(int id)
    {
        var usuario = await _userRepository.BuscarPorIdAsync(id);

        if (usuario == null)
            return null;

        return _mapper.Map<UserResponse>(usuario);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var usuario = await _userRepository.BuscarPorIdAsync(id);
        if (usuario == null)
            return false;

        return await _userRepository.DeleteAsync(usuario); 
    }

    public async Task<UserResponse> AtualizarPerfilAsync(int id, AtualizarUserRequest request)
    {
        var usuario = await _userRepository.BuscarPorIdAsync(id);
        if (usuario == null)
            throw new InvalidOperationException("Usuário não encontrado.");

        _mapper.Map(request, usuario);

        if (!string.IsNullOrWhiteSpace(request.Password))
            usuario.SetPassword(request.Password);

        await _userRepository.AtualizarAsync(usuario);


        return _mapper.Map<UserResponse>(usuario);
    }
    
}
