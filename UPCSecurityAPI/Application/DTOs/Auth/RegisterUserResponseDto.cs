namespace UPCSecurityAPI.Application.DTOs.Auth;

public class RegisterUserResponseDto
{
    public string Message { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
