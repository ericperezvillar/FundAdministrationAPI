using FundAdmin.Application.DTOs;

namespace FundAdmin.Application.Interfaces.Services;
public interface IInvestorService
{
    Task<IEnumerable<InvestorReadDto>> GetAllAsync();
    Task<InvestorReadDto?> GetByIdAsync(int id);
    Task<InvestorReadDto> CreateAsync(InvestorCreateDto dto);
    Task<bool> UpdateAsync(int id, InvestorUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
