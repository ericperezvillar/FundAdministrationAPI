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
            var dto = new FundReadDto(5, "Alpha", "USD", new DateTime(2020, 1, 1));
            _mockService.Setup(s => s.GetByIdAsync(5)).ReturnsAsync(dto);

            var result = await _controller.GetById(5);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, ok.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNull()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((FundReadDto?)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            var updateDto = new FundUpdateDto("Updated Fund", "EUR", new DateTime(2024, 1, 1));
            _mockService.Setup(s => s.UpdateAsync(1, updateDto)).ReturnsAsync(true);

            var result = await _controller.Update(1, updateDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            var updateDto = new FundUpdateDto("Nonexistent Fund", "USD", new DateTime(2024, 1, 1));
            _mockService.Setup(s => s.UpdateAsync(999, updateDto)).ReturnsAsync(false);

            var result = await _controller.Update(999, updateDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenTrue()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            var createDto = new FundCreateDto("Alpha", "USD", new DateTime(2020, 1, 1));
            var readDto = new FundReadDto(1, "Alpha", "USD", new DateTime(2020, 1, 1));

            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(readDto);

            var result = await _controller.Create(createDto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(readDto, created.Value);
            Assert.Equal(nameof(_controller.GetById), created.ActionName);
        }
    }

}
