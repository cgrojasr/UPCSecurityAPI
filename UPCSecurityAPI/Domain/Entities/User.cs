namespace UPCSecurityAPI.Domain.Entities;

public class User
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string NroDocumento { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
