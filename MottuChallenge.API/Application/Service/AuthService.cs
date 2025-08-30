using AutoMapper;
using FluentValidation;
using MottuChallenge.API.Domain.Dtos.Request;
using MottuChallenge.API.Domain.Dtos.Response;
using MottuChallenge.API.Domain.Interfaces;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Application.Service;

public class AuthService
{

    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IValidator<UserCreateRequest> _validator;
    
    public AuthService(IUserRepository usuarioRepository, TokenService tokenService, IMapper mapper, IValidator<UserCreateRequest> validator)
    {
        _userRepository = usuarioRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _validator = validator;

    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {

        var usuario = await _userRepository.BuscarPorEmailAsync(request.Email);

        if (usuario == null)
            throw new Exception("Usuário não encontrado.");
        
        if (!usuario.CheckPassword(request.Password))
            throw new Exception("Senha inválida.");
        
        var token = _tokenService.Generate(usuario);
        
        return new LoginResponse
        {
            Token = token,
            Email = usuario.Email,
            Nome = usuario.Nome
        };
    }

    public async Task<UserResponse> CreateUserAsync(UserCreateRequest userRequest)
    {
        var result = await _validator.ValidateAsync(userRequest);

        if (!result.IsValid)
        {
            var errors = result.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage} ")
                .ToList();

            throw new ValidationException(string.Join(Environment.NewLine, errors));
        }

        if (await _userRepository.VerificaEmailExisteAsync(userRequest.Email))
            throw new InvalidOperationException("Email já cadastrado.");

        var usuario = _mapper.Map<User>(userRequest);
        usuario.SetPassword(userRequest.Password);
        var usuarioCriado = await _userRepository.CriarAsync(usuario);

        return _mapper.Map<UserResponse>(usuarioCriado);
    }
}