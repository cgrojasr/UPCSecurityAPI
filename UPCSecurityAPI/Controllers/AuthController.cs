using Microsoft.AspNetCore.Mvc;
using UPCSecurityAPI.Application.DTOs.Auth;
using UPCSecurityAPI.Application.Interfaces.Services;
using UPCSecurityAPI.CrossCutting.Exceptions;
using UPCSecurityAPI.CrossCutting.Responses;

namespace UPCSecurityAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterUserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.RegisterAsync(request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (AppValidationException ex)
        {
            return BadRequest(new ErrorResponse
            {
                Message = ex.Message,
                Errors = ex.Errors
            });
        }
        catch (ResourceConflictException ex)
        {
            return Conflict(new ErrorResponse
            {
                Message = ex.Message
            });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.LoginAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (AppValidationException ex)
        {
            return BadRequest(new ErrorResponse
            {
                Message = ex.Message,
                Errors = ex.Errors
            });
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(new ErrorResponse
            {
                Message = ex.Message
            });
        }
    }
}
