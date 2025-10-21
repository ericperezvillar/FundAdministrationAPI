using FundAdmin.Application.DTOs;

namespace FundAdmin.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> AuthenticateAsync(LoginRequestDto dto);
}
