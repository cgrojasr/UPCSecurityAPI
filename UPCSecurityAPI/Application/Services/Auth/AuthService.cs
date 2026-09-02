using System.Net.Mail;
using UPCSecurityAPI.Application.DTOs.Auth;
using UPCSecurityAPI.Application.Interfaces.Services;
using UPCSecurityAPI.CrossCutting.Exceptions;
using UPCSecurityAPI.Domain.Entities;
using UPCSecurityAPI.Domain.Interfaces.Adapters;
using UPCSecurityAPI.Domain.Interfaces.Repositories;

namespace UPCSecurityAPI.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenBuilder _jwtTokenBuilder;

    public AuthService(IUserRepository userRepository, IJwtTokenBuilder jwtTokenBuilder)
    {
        _userRepository = userRepository;
        _jwtTokenBuilder = jwtTokenBuilder;
    }

    public async Task<RegisterUserResponseDto> RegisterAsync(RegisterUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var email = (request.Email?.Trim() ?? string.Empty).ToLowerInvariant();
        var password = request.Password ?? string.Empty;

        ValidateRequest(email, password);

        var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (existingUser is not null)
        {
            throw new ResourceConflictException("El usuario ya existe.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user, cancellationToken);

        return new RegisterUserResponseDto
        {
            Message = "Usuario registrado exitosamente.",
            UserId = user.Id,
            Email = user.Email
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var email = (request.Email?.Trim() ?? string.Empty).ToLowerInvariant();
        var password = request.Password ?? string.Empty;

        ValidateLoginRequest(email, password);

        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new InvalidCredentialsException("Credenciales inválidas.");
        }

        var token = _jwtTokenBuilder.BuildToken(user.Id, user.Email, user.Role);

        return new LoginResponseDto
        {
            AccessToken = token.AccessToken,
            TokenType = "Bearer",
            ExpiresAtUtc = token.ExpiresAtUtc
        };
    }

    private static void ValidateRequest(string email, string password)
    {
        var errors = new Dictionary<string, string[]>();

        if (!IsValidEmail(email))
        {
            errors["email"] = ["El email es inválido."];
        }

        if (!IsValidPassword(password))
        {
            errors["password"] =
            [
                "La contraseña debe tener al menos 8 caracteres e incluir letras y números."
            ];
        }

        if (errors.Count > 0)
        {
            throw new AppValidationException(errors);
        }
    }

    private static void ValidateLoginRequest(string email, string password)
    {
        var errors = new Dictionary<string, string[]>();

        if (!IsValidEmail(email))
        {
            errors["email"] = ["El email es inválido."];
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            errors["password"] = ["La contraseña es requerida."];
        }

        if (errors.Count > 0)
        {
            throw new AppValidationException(errors);
        }
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var address = new MailAddress(email);
            return address.Address.Equals(email, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            return false;
        }

        var hasLetter = password.Any(char.IsLetter);
        var hasDigit = password.Any(char.IsDigit);
        return hasLetter && hasDigit;
    }
}
