using FundAdmin.Application.DTOs;

namespace FundAdmin.Application.Interfaces.Services;
public interface IFundService
{
    Task<IEnumerable<FundReadDto>> GetAllAsync();
    Task<FundReadDto?> GetByIdAsync(Guid id);
    Task<FundReadDto> CreateAsync(FundCreateDto dto);
    Task<bool> UpdateAsync(Guid id, FundUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}
