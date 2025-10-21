using FundAdmin.Application.DTOs;

namespace FundAdmin.Application.Interfaces.Services;

public interface IReportingService
{
    Task<IEnumerable<FundInvestmentSummaryDto>> GetFundSummariesAsync();
}
