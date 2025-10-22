using FundAdmin.Domain.Entities;

namespace FundAdmin.Application.Interfaces.Repositories;
public interface IInvestorRepository
{
    Task<IEnumerable<Investor>> GetAllAsync();
    Task<Investor?> GetByIdAsync(Guid id);
    Task<Investor> AddAsync(Investor entity);
    Task UpdateAsync(Investor entity);
    Task DeleteAsync(Investor entity);
    Task<bool> ExistsAsync(Guid id);
    Task<IEnumerable<Investor>> GetByFundAsync(Guid fundId);
}
