using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Domain.Entities;
using FundAdmin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FundAdmin.Infrastructure.Repositories.Implementations;

public class FundRepository : IFundRepository
{
    private readonly AppDbContext _db;
    public FundRepository(AppDbContext db) => _db = db;

    public async Task<Fund> AddAsync(Fund entity)
    {
        _db.Funds.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Fund entity)
    {
        _db.Funds.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id) => await _db.Funds.AnyAsync(x => x.FundId == id);

    public async Task<IEnumerable<Fund>> GetAllAsync() => await _db.Funds.AsNoTracking().ToListAsync();

    public async Task<Fund?> GetByIdAsync(int id) => await _db.Funds.AsNoTracking().FirstOrDefaultAsync(x => x.FundId == id);

    public async Task UpdateAsync(Fund entity)
    {
        _db.Funds.Update(entity);
        await _db.SaveChangesAsync();
    }
}
