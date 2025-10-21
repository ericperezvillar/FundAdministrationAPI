using Xunit;
using Microsoft.EntityFrameworkCore;
using FundAdmin.Infrastructure.Repositories.Implementations;
using FundAdmin.Domain.Entities;
using FundAdmin.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using System.Linq;


namespace FundAdmin.Infrastructure.Tests.Repositories
{   
    public class FundRepositoryTests
    {
        private static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddFund()
        {
            using var context = CreateContext();
            var repo = new FundRepository(context);
            var fund = new Fund { FundName = "Alpha", CurrencyCode = "USD", LaunchDate = DateTime.UtcNow };

            await repo.AddAsync(fund);

            Assert.Equal(1, await context.Funds.CountAsync());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnFund()
        {
            using var context = CreateContext();
            var fund = new Fund { FundName = "Test", CurrencyCode = "EUR" };
            context.Funds.Add(fund);
            await context.SaveChangesAsync();

            var repo = new FundRepository(context);
            var result = await repo.GetByIdAsync(fund.FundId);

            Assert.NotNull(result);
            Assert.Equal("Test", result!.FundName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllFunds()
        {
            using var context = CreateContext();
            context.Funds.AddRange(
                new Fund { FundName = "F1", CurrencyCode = "USD" },
                new Fund { FundName = "F2", CurrencyCode = "EUR" });
            await context.SaveChangesAsync();

            var repo = new FundRepository(context);
            var result = await repo.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenFundExists()
        {
            using var context = CreateContext();
            context.Funds.Add(new Fund { FundName = "Exists", CurrencyCode = "GBP" });
            await context.SaveChangesAsync();

            var repo = new FundRepository(context);
            var exists = await repo.ExistsAsync(1);

            Assert.True(exists);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveFund()
        {
            using var context = CreateContext();
            var fund = new Fund { FundName = "DeleteMe", CurrencyCode = "USD" };
            context.Funds.Add(fund);
            await context.SaveChangesAsync();

            var repo = new FundRepository(context);
            await repo.DeleteAsync(fund);

            Assert.Empty(context.Funds);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyFund()
        {
            using var context = CreateContext();
            var fund = new Fund { FundName = "Old", CurrencyCode = "USD" };
            context.Funds.Add(fund);
            await context.SaveChangesAsync();

            fund.FundName = "Updated";
            var repo = new FundRepository(context);
            await repo.UpdateAsync(fund);

            var updated = await context.Funds.FirstAsync();
            Assert.Equal("Updated", updated.FundName);
        }
    }

}
