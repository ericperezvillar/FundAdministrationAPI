using Moq;
using AutoMapper;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Application.Services;
using FundAdmin.Domain.Entities;
using FundAdmin.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FundAdmin.Application.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _txRepo;
        private readonly Mock<IFundRepository> _fundRepo;
        private readonly Mock<IInvestorRepository> _investorRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<TransactionService>> _logger;
        private readonly TransactionService _service;

        public TransactionServiceTests()
        {
            _txRepo = new Mock<ITransactionRepository>();
            _fundRepo = new Mock<IFundRepository>();
            _investorRepo = new Mock<IInvestorRepository>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<TransactionService>>();
            _service = new TransactionService(_txRepo.Object, _fundRepo.Object, _investorRepo.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenInvestorNotFound()
        {
            _investorRepo.Setup(r => r.GetByIdAsync(Guid.NewGuid())).ReturnsAsync((Investor?)null);
            var dto = new TransactionCreateDto(Guid.NewGuid(), TransactionType.Subscription, 500, DateTime.UtcNow);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task GetFundSummaryAsync_ShouldReturnNull_WhenFundNotFound()
        {
            _fundRepo.Setup(r => r.GetByIdAsync(Guid.NewGuid())).ReturnsAsync((Fund?)null);

            var result = await _service.GetFundSummaryAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetFundSummaryAsync_ShouldReturnDto_WhenFundExists()
        {
            var fundId = Guid.NewGuid();
            var fund = new Fund { FundId = fundId, FundName = "Alpha", CurrencyCode = "USD" };

            _fundRepo.Setup(r => r.GetByIdAsync(fundId)).ReturnsAsync(fund);
            _txRepo.Setup(r => r.GetTotalByFundAsync(fundId, TransactionType.Subscription)).ReturnsAsync(500);
            _txRepo.Setup(r => r.GetTotalByFundAsync(fundId, TransactionType.Redemption)).ReturnsAsync(200);

            var result = await _service.GetFundSummaryAsync(fundId);

            Assert.NotNull(result);
            Assert.Equal(300, result!.NetInvestment);
            Assert.Equal("Alpha", result.FundName);
        }
    }

}
