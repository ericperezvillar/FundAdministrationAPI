using FundAdmin.Domain.Entities;
using FundAdmin.Domain.Enums;

namespace FundAdmin.Application.Interfaces.Repositories;
public interface ITransactionRepository
{
    Task<Transaction> AddAsync(Transaction entity);
    Task<IEnumerable<Transaction>> GetByInvestorAsync(int investorId);
    Task<decimal> GetTotalByFundAsync(int fundId, TransactionType subscriptions);
}
