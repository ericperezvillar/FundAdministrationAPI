using FundAdmin.Domain.Entities;

namespace FundAdmin.Application.Interfaces.Repositories;
public interface IInvestorRepository
{
    Task<IEnumerable<Investor>> GetAllAsync();
    Task<Investor?> GetByIdAsync(int id);
    Task<Investor> AddAsync(Investor entity);
    Task UpdateAsync(Investor entity);
    Task DeleteAsync(Investor entity);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Investor>> GetByFundAsync(int fundId);
}
