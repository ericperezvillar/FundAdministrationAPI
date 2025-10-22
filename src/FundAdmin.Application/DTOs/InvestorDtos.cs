using FundAdmin.Application.Interfaces.Dtos;

namespace FundAdmin.Application.DTOs;

public record InvestorCreateDto(string FullName, string Email, Guid FundId) : IInvestorBase;
public record InvestorUpdateDto(string FullName, string Email, Guid FundId) : IInvestorBase;
public record InvestorReadDto(Guid InvestorId, string FullName, string Email, Guid FundId);
