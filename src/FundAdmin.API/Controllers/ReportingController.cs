using Asp.Versioning;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundAdmin.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/reporting")]
[Authorize]
public class ReportingController : ControllerBase
{
    private readonly IReportingService _reportingService;

    public ReportingController(IReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    /// <summary>
    /// Report that summarize Funding and Investments
    /// </summary>
    /// <returns>Net investment per fund and the Number of investors per fund</returns>
    [HttpGet("net-investments")]
    public async Task<ActionResult<IEnumerable<FundNetInvestmentDto>>> GetNetInvestments()
    {
        return Ok(await _reportingService.GetFundSummariesAsync());
    }
}
