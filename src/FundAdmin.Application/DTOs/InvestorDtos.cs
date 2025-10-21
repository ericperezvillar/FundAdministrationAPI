using FundAdmin.Application.Interfaces.Dtos;

namespace FundAdmin.Application.DTOs;

public record InvestorCreateDto(string FullName, string Email, int FundId) : IInvestorBase;
public record InvestorUpdateDto(string FullName, string Email, int FundId) : IInvestorBase;
public record InvestorReadDto(int InvestorId, string FullName, string Email, int FundId);
