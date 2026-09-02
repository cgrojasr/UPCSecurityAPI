namespace UPCSecurityAPI.Domain.Interfaces.Adapters;

public interface IJwtTokenBuilder
{
    JwtTokenResult BuildToken(string userId, string email, string role);
}
