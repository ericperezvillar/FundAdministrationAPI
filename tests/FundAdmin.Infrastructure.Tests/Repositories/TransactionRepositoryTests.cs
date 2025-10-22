using Microsoft.EntityFrameworkCore;
using FundAdmin.Infrastructure.Repositories.Implementations;
using FundAdmin.Domain.Entities;
using FundAdmin.Domain.Enums;
using FundAdmin.Infrastructure.Data;

namespace FundAdmin.Infrastructure.Tests.Repositories
{
    public class TransactionRepositoryTests
    {
        private static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddTransaction()
        {
            using var context = CreateContext();
            var repo = new TransactionRepository(context);
            var tx = new Transaction
            {
                InvestorId = Guid.NewGuid(),
                Amount = 100,
                Type = TransactionType.Subscription,
                TransactionDate = DateTime.UtcNow
            };

            await repo.AddAsync(tx);

            Assert.Single(context.Transactions);
        }

        [Fact]
        public async Task GetByInvestorAsync_ShouldReturnTransactions()
        {
            var investorId = Guid.NewGuid();
            using var context = CreateContext();
            context.Transactions.AddRange(
                new Transaction { InvestorId = investorId, Amount = 100, Type = TransactionType.Subscription, TransactionDate = DateTime.UtcNow },
                new Transaction { InvestorId = Guid.NewGuid(), Amount = 200, Type = TransactionType.Subscription, TransactionDate = DateTime.UtcNow });
            await context.SaveChangesAsync();

            var repo = new TransactionRepository(context);
            var result = await repo.GetByInvestorAsync(investorId);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetTotalByFundAsync_ShouldReturnSum()
        {
            var fundId = Guid.NewGuid();
            var investorId = Guid.NewGuid();
            using var context = CreateContext();
            var investor = new Investor { InvestorId = investorId, FundId = fundId };
            context.Investors.Add(investor);
            context.Transactions.AddRange(
                new Transaction { InvestorId = investorId, Investor = investor, Amount = 100, Type = TransactionType.Subscription },
                new Transaction { InvestorId = investorId, Investor = investor, Amount = 50, Type = TransactionType.Subscription }
            );
            await context.SaveChangesAsync();

            var repo = new TransactionRepository(context);
            var total = await repo.GetTotalByFundAsync(fundId, TransactionType.Subscription);

            Assert.Equal(150, total);
        }
    }

}
