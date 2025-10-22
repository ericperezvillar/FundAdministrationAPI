using Asp.Versioning;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundAdmin.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/investors")]
[Authorize]
public class InvestorsController : ControllerBase
{
    private readonly IInvestorService _service;
    public InvestorsController(IInvestorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvestorReadDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<InvestorReadDto>> GetById(Guid id)
    {
        var investor = await _service.GetByIdAsync(id);
        return investor is null ? NotFound() : Ok(investor);
    }

    [HttpPost]
    public async Task<ActionResult<InvestorReadDto>> Create(InvestorCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.InvestorId }, created);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, InvestorUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
