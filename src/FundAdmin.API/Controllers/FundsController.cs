using Asp.Versioning;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundAdmin.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/funds")]
[Authorize]
public class FundsController : ControllerBase
{
    private readonly IFundService _service;
    public FundsController(IFundService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FundReadDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FundReadDto>> GetById(int id)
    {
        var fund = await _service.GetByIdAsync(id);
        return fund is null ? NotFound() : Ok(fund);
    }
    
    [HttpPost]
    public async Task<ActionResult<FundReadDto>> Create(FundCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.FundId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, FundUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
