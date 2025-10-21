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
            var investor = new InvestorReadDto(1, "John Doe", "john@example.com", 2);
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(investor);

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(investor, ok.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNull()
        {
            _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((InvestorReadDto?)null);

            var result = await _controller.GetById(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccess()
        {
            _mockService.Setup(s => s.UpdateAsync(1, It.IsAny<InvestorUpdateDto>())).ReturnsAsync(true);

            var dto = new InvestorUpdateDto("Jane Doe", "jane@example.com", 2);
            var result = await _controller.Update(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            _mockService.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

            var result = await _controller.Delete(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            var createDto = new InvestorCreateDto("Jane Doe", "jane@example.com", 2);
            var readDto = new InvestorReadDto(1, "Jane Doe", "jane@example.com", 2);

            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(readDto);

            var result = await _controller.Create(createDto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(readDto, created.Value);
            Assert.Equal(nameof(_controller.GetById), created.ActionName);
        }
    }

}
