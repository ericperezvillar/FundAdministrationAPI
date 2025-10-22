using Asp.Versioning;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundAdmin.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    public TransactionsController(ITransactionService service)
    {
        _transactionService = service;
    }

    /// <summary>
    /// Create Transaction
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<TransactionReadDto>> Create(TransactionCreateDto dto)
    {
        var created = await _transactionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetByInvestor), new { investorId = created.InvestorId }, created);
    }

    /// <summary>
    /// Get transactions per Investor
    /// </summary>
    /// <param name="investorId">Investor Id</param>
    /// <returns>List of transactions</returns>
    [HttpGet("investor/{investorId:int}")]
    public async Task<ActionResult<IEnumerable<TransactionReadDto>>> GetByInvestor(int investorId)
    {
        return Ok(await _transactionService.GetByInvestorAsync(investorId));
    }

    /// <summary>
    /// Get transactions per Fund
    /// </summary>
    /// <param name="fundId">Fund Id</param>
    /// <returns>List of transactions</returns>
    [HttpGet("fund/{fundId:int}/summary")]
    public async Task<ActionResult<IEnumerable<TransactionReadDto>>> GetFundTransactionSummary(int fundId)
    {
        var summary = await _transactionService.GetFundSummaryAsync(fundId);

        if (summary is null)
            return NotFound(new { message = $"Fund with ID {fundId} not found." });

        return Ok(summary);
    }
}
