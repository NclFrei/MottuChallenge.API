using Microsoft.Extensions.Options;
using MottuChallenge.API.Domain.Interfaces;

namespace MottuChallenge.API.Infrastructure.Configuration;

public class JwtSettingsProvider : IJwtSettingsProvider
{
    private readonly JwtSettings _jwtSettings;

    public JwtSettingsProvider(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string SecretKey => _jwtSettings.SecretKey;

}