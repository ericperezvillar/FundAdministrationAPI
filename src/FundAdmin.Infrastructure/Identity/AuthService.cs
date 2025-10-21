using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FundAdmin.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;

    private static readonly Dictionary<string, string> Users = new()
    {
        { "eric", "password123" },
        { "admin", "admin123" }
    };

    public AuthService(IConfiguration config) => _config = config;

    public Task<AuthResponseDto?> AuthenticateAsync(LoginRequestDto dto)
    {
        if (!Users.TryGetValue(dto.Username, out var storedPwd) || storedPwd != dto.Password)
            return Task.FromResult<AuthResponseDto?>(null);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, dto.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, dto.Username)
        };

        var expires = DateTime.UtcNow.AddMinutes(60);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult<AuthResponseDto?>(new AuthResponseDto(jwt, expires));
    }
}
