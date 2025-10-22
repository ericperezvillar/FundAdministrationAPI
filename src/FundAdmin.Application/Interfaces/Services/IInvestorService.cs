using FundAdmin.Application.DTOs;

namespace FundAdmin.Application.Interfaces.Services;
public interface IInvestorService
{
    Task<IEnumerable<InvestorReadDto>> GetAllAsync();
    Task<InvestorReadDto?> GetByIdAsync(Guid id);
    Task<InvestorReadDto> CreateAsync(InvestorCreateDto dto);
    Task<bool> UpdateAsync(Guid id, InvestorUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}
