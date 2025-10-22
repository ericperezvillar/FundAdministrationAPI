using FundAdmin.Domain.Entities;

namespace FundAdmin.Application.Interfaces.Repositories;
public interface IFundRepository
{
    Task<IEnumerable<Fund>> GetAllAsync();
    Task<Fund?> GetByIdAsync(Guid id);
    Task<Fund> AddAsync(Fund entity);
    Task UpdateAsync(Fund entity);
    Task DeleteAsync(Fund entity);
    Task<bool> ExistsAsync(Guid id);
}
