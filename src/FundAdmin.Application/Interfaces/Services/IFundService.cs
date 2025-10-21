using FundAdmin.Application.DTOs;

namespace FundAdmin.Application.Interfaces.Services;
public interface IFundService
{
    Task<IEnumerable<FundReadDto>> GetAllAsync();
    Task<FundReadDto?> GetByIdAsync(int id);
    Task<FundReadDto> CreateAsync(FundCreateDto dto);
    Task<bool> UpdateAsync(int id, FundUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
