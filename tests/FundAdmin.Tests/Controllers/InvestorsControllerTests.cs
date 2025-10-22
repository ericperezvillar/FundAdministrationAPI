using Moq;
using FundAdmin.API.Controllers;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FundAdmin.API.Tests.Controllers
{
    public class InvestorsControllerTests
    {
        private readonly Mock<IInvestorService> _mockService;
        private readonly InvestorsController _controller;

        public InvestorsControllerTests()
        {
            _mockService = new Mock<IInvestorService>();
            _controller = new InvestorsController(_mockService.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            var investor = new InvestorReadDto(Guid.NewGuid(), "John Doe", "john@example.com", Guid.NewGuid());
            _mockService.Setup(s => s.GetByIdAsync(investor.InvestorId)).ReturnsAsync(investor);

            var result = await _controller.GetById(investor.InvestorId);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(investor, ok.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNull()
        {
            var investorId = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(investorId)).ReturnsAsync((InvestorReadDto?)null);

            var result = await _controller.GetById(investorId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccess()
        {
            var investorId = Guid.NewGuid();
            _mockService.Setup(s => s.UpdateAsync(investorId, It.IsAny<InvestorUpdateDto>())).ReturnsAsync(true);

            var dto = new InvestorUpdateDto("Jane Doe", "jane@example.com", Guid.NewGuid());
            var result = await _controller.Update(investorId, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            var investorId = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(investorId)).ReturnsAsync(true);

            var result = await _controller.Delete(investorId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            _mockService.Setup(s => s.DeleteAsync(Guid.NewGuid())).ReturnsAsync(false);

            var result = await _controller.Delete(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            var createDto = new InvestorCreateDto("Jane Doe", "jane@example.com", Guid.NewGuid());
            var readDto = new InvestorReadDto(Guid.NewGuid(), "Jane Doe", "jane@example.com", Guid.NewGuid());

            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(readDto);

            var result = await _controller.Create(createDto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(readDto, created.Value);
            Assert.Equal(nameof(_controller.GetById), created.ActionName);
        }
    }

}
