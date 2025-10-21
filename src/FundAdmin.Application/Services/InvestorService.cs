using AutoMapper;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Application.Interfaces.Services;
using FundAdmin.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FundAdmin.Application.Services;

public class InvestorService : IInvestorService
{
    private readonly IInvestorRepository _investorRepo;
    private readonly ITransactionRepository _transactionRepo;
    private readonly IFundRepository _fundRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<InvestorService> _logger;

    public InvestorService(IInvestorRepository investorRepo, ITransactionRepository transactionRepo, IFundRepository fundRepo, IMapper mapper, ILogger<InvestorService> logger)
    {
        _investorRepo = investorRepo;
        _transactionRepo = transactionRepo;
        _fundRepo = fundRepo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<InvestorReadDto> CreateAsync(InvestorCreateDto dto)
    {
        var fundExists = await _fundRepo.ExistsAsync(dto.FundId);
        if (!fundExists)
        {
            _logger.LogWarning("Attempted to create investor for non-existing FundId={FundId}", dto.FundId);
            throw new ArgumentException($"Fund with ID {dto.FundId} does not exist.");
        }

        var investor = _mapper.Map<Investor>(dto);
        await _investorRepo.AddAsync(investor);
        return _mapper.Map<InvestorReadDto>(investor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _investorRepo.GetByIdAsync(id);
        if (entity is null) return false;
        await _investorRepo.DeleteAsync(entity);
        return true;
    }

    public async Task<IEnumerable<InvestorReadDto>> GetAllAsync()
    {
        var items = await _investorRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<InvestorReadDto>>(items);
    }

    public async Task<InvestorReadDto?> GetByIdAsync(int id)
    {
        var item = await _investorRepo.GetByIdAsync(id);
        return item is null ? null : _mapper.Map<InvestorReadDto>(item);
    }

    public async Task<bool> UpdateAsync(int id, InvestorUpdateDto dto)
    {
        var fundExists = await _fundRepo.ExistsAsync(dto.FundId);
        if (!fundExists)
        {
            _logger.LogWarning("Attempted to create investor for non-existing FundId={FundId}", dto.FundId);
            throw new ArgumentException($"Fund with ID {dto.FundId} does not exist.");
        }

        var investor = await _investorRepo.GetByIdAsync(id);
        if (investor == null)
            return false;

        _mapper.Map(dto, investor);
        await _investorRepo.UpdateAsync(investor);
        return true;
    }
}
