using Microsoft.EntityFrameworkCore;
using FundAdmin.Infrastructure.Repositories.Implementations;
using FundAdmin.Domain.Entities;
using FundAdmin.Infrastructure.Data;

namespace FundAdmin.Infrastructure.Tests.Repositories
{    
    public class InvestorRepositoryTests
    {
        private static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddInvestor()
        {
            using var context = CreateContext();
            var repo = new InvestorRepository(context);
            var investor = new Investor { FullName = "John Doe", Email = "john@test.com", FundId = 1 };

            await repo.AddAsync(investor);

            Assert.Single(context.Investors);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnInvestor()
        {
            using var context = CreateContext();
            var investor = new Investor { FullName = "Jane", Email = "jane@test.com", FundId = 1 };
            context.Investors.Add(investor);
            await context.SaveChangesAsync();

            var repo = new InvestorRepository(context);
            var result = await repo.GetByIdAsync(investor.InvestorId);

            Assert.NotNull(result);
            Assert.Equal("Jane", result!.FullName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllInvestors()
        {
            using var context = CreateContext();
            context.Investors.AddRange(
                new Investor { FullName = "I1", Email = "a@a.com", FundId = 1 },
                new Investor { FullName = "I2", Email = "b@b.com", FundId = 2 });
            await context.SaveChangesAsync();

            var repo = new InvestorRepository(context);
            var result = await repo.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenInvestorExists()
        {
            using var context = CreateContext();
            context.Investors.Add(new Investor { FullName = "Exist", Email = "exist@test.com", FundId = 1 });
            await context.SaveChangesAsync();

            var repo = new InvestorRepository(context);
            var exists = await repo.ExistsAsync(1);

            Assert.True(exists);
        }

        [Fact]
        public async Task GetByFundAsync_ShouldReturnInvestorsByFund()
        {
            using var context = CreateContext();
            context.Investors.AddRange(
                new Investor { FullName = "I1", FundId = 1 },
                new Investor { FullName = "I2", FundId = 2 });
            await context.SaveChangesAsync();

            var repo = new InvestorRepository(context);
            var result = await repo.GetByFundAsync(1);

            Assert.Single(result);
            Assert.Equal("I1", result.First().FullName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyInvestor()
        {
            using var context = CreateContext();
            var investor = new Investor { FullName = "Old", FundId = 1 };
            context.Investors.Add(investor);
            await context.SaveChangesAsync();

            investor.FullName = "Updated";
            var repo = new InvestorRepository(context);
            await repo.UpdateAsync(investor);

            var updated = await context.Investors.FirstAsync();
            Assert.Equal("Updated", updated.FullName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveInvestor()
        {
            using var context = CreateContext();
            var investor = new Investor { FullName = "ToDelete", FundId = 1 };
            context.Investors.Add(investor);
            await context.SaveChangesAsync();

            var repo = new InvestorRepository(context);
            await repo.DeleteAsync(investor);

            Assert.Empty(context.Investors);
        }
    }

}
