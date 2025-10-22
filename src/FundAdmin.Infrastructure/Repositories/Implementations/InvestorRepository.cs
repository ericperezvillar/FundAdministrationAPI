using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Domain.Entities;
using FundAdmin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FundAdmin.Infrastructure.Repositories.Implementations;

public class InvestorRepository : IInvestorRepository
{
    private readonly AppDbContext _db;
    public InvestorRepository(AppDbContext db) => _db = db;

    public async Task<Investor> AddAsync(Investor entity)
    {
        _db.Investors.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Investor entity)
    {
        _db.Investors.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id) => await _db.Investors.AnyAsync(x => x.InvestorId == id);

    public async Task<IEnumerable<Investor>> GetAllAsync() => await _db.Investors.AsNoTracking().ToListAsync();

    public async Task<Investor?> GetByIdAsync(Guid id) => await _db.Investors.AsNoTracking().FirstOrDefaultAsync(x => x.InvestorId == id);

    public async Task<IEnumerable<Investor>> GetByFundAsync(Guid fundId) => await _db.Investors.AsNoTracking().Where(i => i.FundId == fundId).ToListAsync();

    public async Task UpdateAsync(Investor entity)
    {
        _db.Investors.Update(entity);
        await _db.SaveChangesAsync();
    }
}
