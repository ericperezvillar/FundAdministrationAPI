using FundAdmin.Domain.Enums;

namespace FundAdmin.Application.DTOs;

public record TransactionCreateDto(Guid InvestorId, TransactionType Type, decimal Amount, DateTime TransactionDate);
public record TransactionReadDto(Guid TransactionId, Guid InvestorId, TransactionType Type, decimal Amount, DateTime TransactionDate);
public record FundTransactionSummaryDto(Guid FundId, string FundName, string CurrencyCode, decimal TotalSubscribed, decimal TotalRedeemed, decimal NetInvestment);
