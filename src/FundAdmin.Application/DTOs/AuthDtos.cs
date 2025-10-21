namespace FundAdmin.Application.DTOs;

public record LoginRequestDto(string Username, string Password);
public record AuthResponseDto(string Token, DateTime ExpiresAt);
