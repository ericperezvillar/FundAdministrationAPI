using Moq;
using Microsoft.Extensions.Logging;
using FundAdmin.Application.Services;
using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Domain.Entities;
using FundAdmin.Domain.Enums;


namespace FundAdmin.Application.Tests.Services
{
    public class ReportingServiceTests
    {
        private readonly Mock<IFundRepository> _fundRepo;
        private readonly Mock<ITransactionRepository> _txRepo;
        private readonly Mock<IInvestorRepository> _investorRepo;
        private readonly Mock<ILogger<ReportingService>> _logger;
        private readonly ReportingService _service;

        public ReportingServiceTests()
        {
            _fundRepo = new Mock<IFundRepository>();
            _txRepo = new Mock<ITransactionRepository>();
            _investorRepo = new Mock<IInvestorRepository>();
            _logger = new Mock<ILogger<ReportingService>>();

            _service = new ReportingService(_fundRepo.Object, _txRepo.Object, _investorRepo.Object, _logger.Object);
        }

        [Fact]
        public async Task GetFundSummariesAsync_ShouldReturnSummaries_ForExistingFunds()
        {
            var funds = new List<Fund>
            {
                new() { FundId = 1, FundName = "Alpha", CurrencyCode = "USD" },
                new() { FundId = 2, FundName = "Beta", CurrencyCode = "EUR" }
            };

            _fundRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(funds);

            _txRepo.Setup(r => r.GetTotalByFundAsync(1, TransactionType.Subscription)).ReturnsAsync(1000);
            _txRepo.Setup(r => r.GetTotalByFundAsync(1, TransactionType.Redemption)).ReturnsAsync(400);
            _txRepo.Setup(r => r.GetTotalByFundAsync(2, TransactionType.Subscription)).ReturnsAsync(500);
            _txRepo.Setup(r => r.GetTotalByFundAsync(2, TransactionType.Redemption)).ReturnsAsync(100);

            _investorRepo.Setup(r => r.GetByFundAsync(1)).ReturnsAsync(new List<Investor> { new(), new() });
            _investorRepo.Setup(r => r.GetByFundAsync(2)).ReturnsAsync(new List<Investor> { new() });

            var result = (await _service.GetFundSummariesAsync()).ToList();

            Assert.Equal(2, result.Count);

            var alpha = result.First(r => r.FundName == "Alpha");
            Assert.Equal("USD", alpha.CurrencyCode);
            Assert.Equal(1000, alpha.SubscribedAmount);
            Assert.Equal(400, alpha.RedeemedAmount);
            Assert.Equal(2, alpha.InvestorCount);

            var beta = result.First(r => r.FundName == "Beta");
            Assert.Equal(500, beta.SubscribedAmount);
            Assert.Equal(100, beta.RedeemedAmount);
            Assert.Equal(1, beta.InvestorCount);

            _fundRepo.Verify(r => r.GetAllAsync(), Times.Once);
            _txRepo.Verify(r => r.GetTotalByFundAsync(It.IsAny<int>(), It.IsAny<TransactionType>()), Times.Exactly(4));
            _investorRepo.Verify(r => r.GetByFundAsync(It.IsAny<int>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetFundSummariesAsync_ShouldReturnEmpty_WhenNoFundsExist()
        {
            _fundRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Fund>());

            var result = await _service.GetFundSummariesAsync();

            Assert.Empty(result);
            _txRepo.Verify(r => r.GetTotalByFundAsync(It.IsAny<int>(), It.IsAny<TransactionType>()), Times.Never);
            _investorRepo.Verify(r => r.GetByFundAsync(It.IsAny<int>()), Times.Never);
        }
    }

}
