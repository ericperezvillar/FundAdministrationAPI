using AutoMapper;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Application.Interfaces.Services;
using FundAdmin.Domain.Entities;
using FundAdmin.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FundAdmin.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepo;
    private readonly IFundRepository _fundRepo;
    private readonly IInvestorRepository _investorRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(ITransactionRepository txRepo, IFundRepository fundRepo, IInvestorRepository investorRepo, IMapper mapper, ILogger<TransactionService> logger)
    {
        _transactionRepo = txRepo; 
        _fundRepo = fundRepo; 
        _investorRepo = investorRepo; 
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TransactionReadDto> CreateAsync(TransactionCreateDto dto)
    {
        _logger.LogInformation($"Creating transaction for Investor={dto.InvestorId}");

        var investor = await _investorRepo.GetByIdAsync(dto.InvestorId);
        if (investor is null) 
            throw new KeyNotFoundException($"Investor {dto.InvestorId} not found");

        var entity = _mapper.Map<Transaction>(dto);
        entity = await _transactionRepo.AddAsync(entity);

        var result = _mapper.Map<TransactionReadDto>(entity);

        _logger.LogInformation($"Transaction created for Investor={dto.InvestorId}");

        return result;
    }

    public async Task<IEnumerable<TransactionReadDto>> GetByInvestorAsync(Guid investorId)
    {
        _logger.LogInformation("Retrieving transaction information for InvestorId={InvestorId}", investorId);

        var items = await _transactionRepo.GetByInvestorAsync(investorId);
        return _mapper.Map<IEnumerable<TransactionReadDto>>(items);
    }

    public async Task<FundTransactionSummaryDto?> GetFundSummaryAsync(Guid fundId)
    {
        _logger.LogInformation("Retrieving transaction summary for FundId={FundId}", fundId);

        var fund = await _fundRepo.GetByIdAsync(fundId);
        if (fund is null)
        {
            _logger.LogWarning("Fund not found for FundId={FundId}", fundId);
            return null;
        }

        var subscribed = await _transactionRepo.GetTotalByFundAsync(fundId, TransactionType.Subscription);
        var redeemed = await _transactionRepo.GetTotalByFundAsync(fundId, TransactionType.Redemption);

        var dto = new FundTransactionSummaryDto(
            fund.FundId,
            fund.FundName,
            fund.CurrencyCode,
            subscribed,
            redeemed,
            subscribed - redeemed
        );

        _logger.LogInformation("Transaction summary generated for FundId={FundId}: Subscribed={Subscribed}, Redeemed={Redeemed}",
            fundId, subscribed, redeemed);

        return dto;
    }

}
