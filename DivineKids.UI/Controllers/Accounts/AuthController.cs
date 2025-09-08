using DivineKids.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DivineKids.Api.Controllers.Accounts;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (request.Password != request.Confirm)
        {
            return BadRequest("Password are not matching");
        }

        var result = await authService.RegisterAsync(request.Name, request.Email, request.Password, request.PhoneNumber);
        if (!result.Succeeded) return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request.Email, request.Password);
        if (!result.Succeeded) return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var result = await authService.RefreshAsync(request.RefreshToken);
        if (!result.Succeeded) return BadRequest(new { error = result.Error });
        return Ok(result.Value);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RefreshRequest request)
    {
        var result = await authService.RevokeAsync(request.RefreshToken);
        if (!result.Succeeded) return BadRequest(new { error = result.Error });
        return NoContent();
    }

    public sealed record RegisterRequest(string Name, string Email, string Password, string Confirm, string PhoneNumber);
    public sealed record LoginRequest(string Email, string Password);
    public sealed record RefreshRequest(string RefreshToken);
}