namespace UPCSecurityAPI.Domain.Interfaces.Adapters;

public class JwtTokenResult
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }
}