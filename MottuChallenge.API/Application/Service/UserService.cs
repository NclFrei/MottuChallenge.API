using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    
    public async Task<PagedResponse<UserResponse>> GetAllUsersAsync( int page, int limit = 10)
    {
        if (page <= 0) page = 1;

        if (limit <= 0 || limit > 100) limit = 10;
        var query = _userRepository.Query();
        
        var total = await query.CountAsync();
        
        var user = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
        
        var userResponse = _mapper.Map<List<UserResponse>>(user);
        
        return new PagedResponse<UserResponse>(
            userResponse,
            page,
            limit,
            total,
            total > page * limit ? $"/api/user?page={page + 1}&limit={limit}" : null,
            page > 1 ? $"/api/user?page={page - 1}&limit={limit}" : null
        );
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
