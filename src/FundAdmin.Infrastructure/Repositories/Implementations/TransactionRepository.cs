using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Domain.Entities;
using FundAdmin.Domain.Enums;
using FundAdmin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FundAdmin.Infrastructure.Repositories.Implementations;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _db;
    public TransactionRepository(AppDbContext db) => _db = db;

    public async Task<Transaction> AddAsync(Transaction entity)
    {
        _db.Transactions.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<Transaction>> GetByInvestorAsync(Guid investorId)
    {
        return await _db.Transactions.AsNoTracking()
            .Where(t => t.InvestorId == investorId)
            .OrderByDescending(t => t.TransactionDate).ToListAsync();
    }

    public async Task<decimal> GetTotalByFundAsync(Guid fundId, TransactionType transactionType)
    {
        var q = _db.Transactions.AsNoTracking()
                    .Where(t => t.Investor!.FundId == fundId && t.Type == transactionType);

        /*  As I was expected to use DECIMAL but SQLite has the limitation of this,
            I needed to validate if the Provider was SQLite in order of avoiding
            loosing data precision. */
        if (_db.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // Client-side aggregation for SQLite problematic DECIMAL handling
            var list = await q.ToListAsync();
            return list.Sum(t => t.Amount);
        }
        return await q.SumAsync(t => t.Amount);
    }
}
