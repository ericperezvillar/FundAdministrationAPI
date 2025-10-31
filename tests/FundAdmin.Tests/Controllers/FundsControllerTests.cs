using Moq;
using FundAdmin.API.Controllers;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FundAdmin.API.Tests.Controllers
{
    public class FundsControllerTests
    {
        private readonly Mock<IFundService> _mockService;
        private readonly FundsController _controller;

        public FundsControllerTests()
        {
            _mockService = new Mock<IFundService>();
            _controller = new FundsController(_mockService.Object);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            var fundId = Guid.NewGuid();
            var dto = new FundReadDto(fundId, "Alpha", "USD", new DateTime(2020, 1, 1));
            _mockService.Setup(s => s.GetByIdAsync(fundId)).ReturnsAsync(dto);

            var result = await _controller.GetById(fundId);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, ok.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNull()
        {
            _mockService.Setup(s => s.GetByIdAsync(Guid.NewGuid())).ReturnsAsync((FundReadDto?)null);

            var result = await _controller.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            var fundId = Guid.NewGuid();
            var updateDto = new FundUpdateDto("Updated Fund", "EUR", new DateTime(2024, 1, 1));
            _mockService.Setup(s => s.UpdateAsync(fundId, updateDto)).ReturnsAsync(true);

            var result = await _controller.Update(fundId, updateDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            var updateDto = new FundUpdateDto("Nonexistent Fund", "USD", new DateTime(2024, 1, 1));
            _mockService.Setup(s => s.UpdateAsync(Guid.NewGuid(), updateDto)).ReturnsAsync(false);

            var result = await _controller.Update(Guid.NewGuid(), updateDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenTrue()
        {
            var fundId = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(fundId)).ReturnsAsync(true);

            var result = await _controller.Delete(fundId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            var createDto = new FundCreateDto("Alpha", "USD", new DateTime(2020, 1, 1));
            var readDto = new FundReadDto(Guid.NewGuid(), "Alpha", "USD", new DateTime(2020, 1, 1));

            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(readDto);

            var result = await _controller.Create(createDto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(readDto, created.Value);
            Assert.Equal(nameof(_controller.GetById), created.ActionName);
        }
    }

}
