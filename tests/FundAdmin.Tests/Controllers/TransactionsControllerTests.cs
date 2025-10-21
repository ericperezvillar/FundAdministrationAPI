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
            var transactions = new List<TransactionReadDto>
            {
                new TransactionReadDto(1, 1, TransactionType.Subscription, 1500, DateTime.UtcNow)
            };

            _mockService.Setup(s => s.GetByInvestorAsync(1)).ReturnsAsync(transactions);

            var result = await _controller.GetByInvestor(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(transactions, ok.Value);
        }

        [Fact]
        public async Task GetFundTransactionSummary_ReturnsNotFound_WhenNull()
        {
            _mockService.Setup(s => s.GetFundSummaryAsync(10)).ReturnsAsync((FundTransactionSummaryDto?)null);

            var result = await _controller.GetFundTransactionSummary(10);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetFundTransactionSummary_ReturnsOk_WhenFound()
        {
            var summary = new FundTransactionSummaryDto(1, "Test", "USD", 500, 200, 300);

            _mockService.Setup(s => s.GetFundSummaryAsync(1)).ReturnsAsync(summary);

            var result = await _controller.GetFundTransactionSummary(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(summary, ok.Value);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            var createDto = new TransactionCreateDto(1, TransactionType.Subscription, 1500, DateTime.UtcNow);
            var readDto = new TransactionReadDto(1, 1, TransactionType.Subscription, 1500, DateTime.UtcNow);

            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(readDto);

            var result = await _controller.Create(createDto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(readDto, created.Value);
            Assert.Equal(nameof(_controller.GetByInvestor), created.ActionName);
        }
    }
}
