using AutoMapper;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Application.Interfaces.Services;
using FundAdmin.Domain.Entities;

namespace FundAdmin.Application;

public class FundService : IFundService
{
    private readonly IFundRepository _fundRepo;
    private readonly IMapper _mapper;
    public FundService(IFundRepository fundRepo, IMapper mapper)
    {
        _fundRepo = fundRepo; _mapper = mapper;
    }

    public async Task<FundReadDto> CreateAsync(FundCreateDto dto)
    {
        var entity = _mapper.Map<Fund>(dto);
        await _fundRepo.AddAsync(entity);
        return _mapper.Map<FundReadDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _fundRepo.GetByIdAsync(id);
        if (entity is null) return false;
        await _fundRepo.DeleteAsync(entity);
        return true;
    }

    public async Task<IEnumerable<FundReadDto>> GetAllAsync()
    {
        var items = await _fundRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<FundReadDto>>(items);
    }

    public async Task<FundReadDto?> GetByIdAsync(int id)
    {
        var item = await _fundRepo.GetByIdAsync(id);
        return item is null ? null : _mapper.Map<FundReadDto>(item);
    }

    public async Task<bool> UpdateAsync(int id, FundUpdateDto dto)
    {
        var entity = await _fundRepo.GetByIdAsync(id);
        if (entity is null) return false;
        entity.FundName = dto.FundName;
        entity.CurrencyCode = dto.CurrencyCode;
        entity.LaunchDate = dto.LaunchDate;
        await _fundRepo.UpdateAsync(entity);
        return true;
    }
}
