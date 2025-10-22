using FundAdmin.API.Controllers;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Services;
using FundAdmin.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FundAdmin.API.Tests.Controllers
{
    public class TransactionsControllerTests
    {
        private readonly Mock<ITransactionService> _mockService;
        private readonly TransactionsController _controller;

        public TransactionsControllerTests()
        {
            _mockService = new Mock<ITransactionService>();
            _controller = new TransactionsController(_mockService.Object);
        }

        [Fact]
        public async Task GetByInvestor_ReturnsOk_WithTransactions()
        {
            var investorId = Guid.NewGuid();
            var transactions = new List<TransactionReadDto>
            {
                new TransactionReadDto(Guid.NewGuid(), investorId, TransactionType.Subscription, 1500, DateTime.UtcNow)
            };

            _mockService.Setup(s => s.GetByInvestorAsync(investorId)).ReturnsAsync(transactions);

            var result = await _controller.GetByInvestor(investorId);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(transactions, ok.Value);
        }

        [Fact]
        public async Task GetFundTransactionSummary_ReturnsNotFound_WhenNull()
        {
            var fundId = Guid.NewGuid();
            _mockService.Setup(s => s.GetFundSummaryAsync(fundId)).ReturnsAsync((FundTransactionSummaryDto?)null);

            var result = await _controller.GetFundTransactionSummary(fundId);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetFundTransactionSummary_ReturnsOk_WhenFound()
        {
            var fundId = Guid.NewGuid();
            var summary = new FundTransactionSummaryDto(fundId, "Test", "USD", 500, 200, 300);

            _mockService.Setup(s => s.GetFundSummaryAsync(fundId)).ReturnsAsync(summary);

            var result = await _controller.GetFundTransactionSummary(fundId);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(summary, ok.Value);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            var createDto = new TransactionCreateDto(Guid.NewGuid(), TransactionType.Subscription, 1500, DateTime.UtcNow);
            var readDto = new TransactionReadDto(Guid.NewGuid(), Guid.NewGuid(), TransactionType.Subscription, 1500, DateTime.UtcNow);

            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(readDto);

            var result = await _controller.Create(createDto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(readDto, created.Value);
            Assert.Equal(nameof(_controller.GetByInvestor), created.ActionName);
        }
    }
}
