namespace FundAdmin.Application.DTOs;

public record FundNetInvestmentDto(string FundName, string CurrencyCode, decimal NetInvestment, int InvestorCount);

public record FundInvestmentSummaryDto(string FundName, string CurrencyCode, decimal SubscribedAmount, decimal RedeemedAmount, int InvestorCount);
