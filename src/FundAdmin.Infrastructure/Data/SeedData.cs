using FundAdmin.Domain.Entities;
using FundAdmin.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FundAdmin.Infrastructure.Data;

public static class SeedData
{
    public static async Task EnsureSeededAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (!await db.Funds.AnyAsync())
        {
            var fund1 = new Fund { FundName = "Global Ventures Fund", CurrencyCode = "USD", LaunchDate = new DateTime(2020,1,1) };
            var fund2 = new Fund { FundName = "Euro Growth Fund", CurrencyCode = "EUR", LaunchDate = new DateTime(2021,6,15) };
            db.Funds.AddRange(fund1, fund2);
            await db.SaveChangesAsync();

            var inv1 = new Investor { FullName = "Alice Johnson", Email = "alice@example.com", FundId = fund1.FundId };
            var inv2 = new Investor { FullName = "Bob Smith", Email = "bob@example.com", FundId = fund1.FundId };
            var inv3 = new Investor { FullName = "Carlos Gomez", Email = "carlos@example.com", FundId = fund2.FundId };
            db.Investors.AddRange(inv1, inv2, inv3);
            await db.SaveChangesAsync();

            db.Transactions.AddRange(
                new Transaction { InvestorId = inv1.InvestorId, Type = TransactionType.Subscription, Amount = 100000, TransactionDate = DateTime.UtcNow.AddDays(-30) },
                new Transaction { InvestorId = inv1.InvestorId, Type = TransactionType.Redemption, Amount = 20000, TransactionDate = DateTime.UtcNow.AddDays(-10) },
                new Transaction { InvestorId = inv2.InvestorId, Type = TransactionType.Subscription, Amount = 50000, TransactionDate = DateTime.UtcNow.AddDays(-5) },
                new Transaction { InvestorId = inv3.InvestorId, Type = TransactionType.Subscription, Amount = 75000, TransactionDate = DateTime.UtcNow.AddDays(-3) }
            );
            await db.SaveChangesAsync();
        }
    }
}
