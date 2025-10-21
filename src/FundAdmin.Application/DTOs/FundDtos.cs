using FundAdmin.Application.Interfaces.Dtos;

namespace FundAdmin.Application.DTOs;

public record FundCreateDto(string FundName, string CurrencyCode, DateTime LaunchDate) : IFundBase;
public record FundUpdateDto(string FundName, string CurrencyCode, DateTime LaunchDate) : IFundBase;
public record FundReadDto(int FundId, string FundName, string CurrencyCode, DateTime LaunchDate);
