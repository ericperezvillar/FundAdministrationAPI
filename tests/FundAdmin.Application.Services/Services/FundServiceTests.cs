using Moq;
using AutoMapper;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Domain.Entities;

namespace FundAdmin.Application.Tests.Services
{
    public class FundServiceTests
    {
        private readonly Mock<IFundRepository> _fundRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly FundService _service;

        public FundServiceTests()
        {
            _fundRepo = new Mock<IFundRepository>();
            _mapper = new Mock<IMapper>();
            _service = new FundService(_fundRepo.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            var funds = new List<Fund>
            {
                new() { FundId = 1, FundName = "Alpha", CurrencyCode = "USD" },
                new() { FundId = 2, FundName = "Beta", CurrencyCode = "EUR" }
            };
            
            var mapped = new List<FundReadDto>
            {
                new(1, "Alpha", "USD", DateTime.UtcNow),
                new(2, "Beta", "EUR", DateTime.UtcNow)
            };

            _fundRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(funds);
            _mapper.Setup(m => m.Map<IEnumerable<FundReadDto>>(funds)).Returns(mapped);

            var result = await _service.GetAllAsync();

            _fundRepo.Verify(r => r.GetAllAsync(), Times.Once);
            Assert.Equal(mapped, result);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedDto()
        {
            var dto = new FundCreateDto("Alpha", "USD", DateTime.UtcNow);
            var entity = new Fund { FundName = "Alpha", CurrencyCode = "USD" };
            var read = new FundReadDto(1, "Alpha", "USD", DateTime.UtcNow);

            _mapper.Setup(m => m.Map<Fund>(dto)).Returns(entity);
            _mapper.Setup(m => m.Map<FundReadDto>(entity)).Returns(read);

            var result = await _service.CreateAsync(dto);

            _fundRepo.Verify(r => r.AddAsync(entity), Times.Once);
            Assert.Equal(read, result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenNotFound()
        {
            _fundRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Fund?)null);
            var dto = new FundUpdateDto("New", "EUR", DateTime.UtcNow);

            var result = await _service.UpdateAsync(99, dto);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenEntityFound()
        {
            var fund = new Fund { FundId = 1 };
            _fundRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fund);

            var result = await _service.DeleteAsync(1);

            Assert.True(result);
            _fundRepo.Verify(r => r.DeleteAsync(fund), Times.Once);
        }
    }

}
