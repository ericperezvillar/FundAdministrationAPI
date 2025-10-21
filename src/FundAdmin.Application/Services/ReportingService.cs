using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Application.Interfaces.Services;
using FundAdmin.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FundAdmin.Application.Services;

public class ReportingService : IReportingService
{
    private readonly IFundRepository _fundRepo;
    private readonly ITransactionRepository _txRepo;
    private readonly IInvestorRepository _investorRepo;
    private readonly ILogger<ReportingService> _logger;

    public ReportingService(
        IFundRepository fundRepo,
        ITransactionRepository txRepo,
        IInvestorRepository investorRepo,
        ILogger<ReportingService> logger)
    {
        _fundRepo = fundRepo;
        _txRepo = txRepo;
        _investorRepo = investorRepo;
        _logger = logger;
    }

    public async Task<IEnumerable<FundInvestmentSummaryDto>> GetFundSummariesAsync()
    {
        _logger.LogInformation("Generating fund investment summary report...");

        var results = new List<FundInvestmentSummaryDto>();
        var allFunds = await _fundRepo.GetAllAsync();

        foreach (var fund in allFunds)
        {
            var subscribed = await _txRepo.GetTotalByFundAsync(fund.FundId, TransactionType.Subscription);
            var redeemed = await _txRepo.GetTotalByFundAsync(fund.FundId, TransactionType.Redemption);
            var investors = await _investorRepo.GetByFundAsync(fund.FundId);

            results.Add(new FundInvestmentSummaryDto (
                fund.FundName,
                fund.CurrencyCode,
                subscribed,
                redeemed,
                investors.Count()
            ));
        }

        _logger.LogInformation("Fund summary report generated successfully with {Count} records.", results.Count);
        return results;
    }
}
