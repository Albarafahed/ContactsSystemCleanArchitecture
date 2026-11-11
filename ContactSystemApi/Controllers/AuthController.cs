
using ContactsSystemApplication.Interfaces;
using ContactsSystemApplication.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ContactsSystemApi.Controllers;

[Route("api/Auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [EnableRateLimiting("RegisterPolicy")]
    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(
            request,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            result);
    }

    [EnableRateLimiting("LoginPolicy")]
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(
            request,
            cancellationToken);

        return Ok(result);
    }


    [EnableRateLimiting("RefreshPolicy")]
    [HttpPost("Refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> RefreshAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RefreshTokenAsync(
            request,
            cancellationToken);

        return Ok(result);
    }

    [EnableRateLimiting("LogoutPolicy")]
    [HttpPost("Logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> LogoutAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        await authService.LogoutAsync(
            request,
            cancellationToken);

        return NoContent();
    }
}
