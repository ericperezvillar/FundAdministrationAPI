using FundAdmin.Application.DTOs;

namespace FundAdmin.Application.Interfaces.Services;
public interface ITransactionService
{
    Task<TransactionReadDto> CreateAsync(TransactionCreateDto dto);
    Task<IEnumerable<TransactionReadDto>> GetByInvestorAsync(Guid investorId);
    Task<FundTransactionSummaryDto?> GetFundSummaryAsync(Guid fundId);
}
