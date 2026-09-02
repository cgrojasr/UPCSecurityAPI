using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UPCSecurityAPI.Domain.Interfaces.Adapters;
using UPCSecurityAPI.Infrastructure.Security.JWT.Options;

namespace UPCSecurityAPI.Infrastructure.Security.JWT.Builders;

public class JwtTokenBuilder : IJwtTokenBuilder
{
    private readonly JwtOptions _options;

    public JwtTokenBuilder(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public JwtTokenResult BuildToken(string userId, string email, string role)
    {
        if (string.IsNullOrWhiteSpace(_options.SecretKey) || _options.SecretKey.Length < 32)
        {
            throw new InvalidOperationException("Jwt:SecretKey debe tener al menos 32 caracteres.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);
        var claims = new[]
        {
            new Claim("userId", userId),
            new Claim("email", email),
            new Claim("role", role),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new JwtTokenResult
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expiresAtUtc
        };
    }
}
