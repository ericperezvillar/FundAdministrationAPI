using Asp.Versioning;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FundAdmin.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>
    /// Login endpoint
    /// </summary>
    /// <param name="dto">User and Password</param>
    /// <returns>Token and expired date</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto dto)
    {
        var result = await _authService.AuthenticateAsync(dto);
        if (result == null)
            return Unauthorized("Invalid username or password");
        return Ok(result);
    }
}
