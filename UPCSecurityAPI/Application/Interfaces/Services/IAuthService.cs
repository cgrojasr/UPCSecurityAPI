using UPCSecurityAPI.Application.DTOs.Auth;

namespace UPCSecurityAPI.Application.Interfaces.Services;

public interface IAuthService
{
    Task<RegisterUserResponseDto> RegisterAsync(RegisterUserRequestDto request, CancellationToken cancellationToken = default);

    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}
