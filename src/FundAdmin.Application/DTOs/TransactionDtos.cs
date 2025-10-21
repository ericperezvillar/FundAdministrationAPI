using FundAdmin.Domain.Enums;

namespace FundAdmin.Application.DTOs;

public record TransactionCreateDto(int InvestorId, TransactionType Type, decimal Amount, DateTime TransactionDate);
public record TransactionReadDto(int TransactionId, int InvestorId, TransactionType Type, decimal Amount, DateTime TransactionDate);
public record FundTransactionSummaryDto(int FundId, string FundName, string CurrencyCode, decimal TotalSubscribed, decimal TotalRedeemed, decimal NetInvestment);
