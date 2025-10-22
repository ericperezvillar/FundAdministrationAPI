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
            var fundId1 = Guid.NewGuid();
            var fundId2 = Guid.NewGuid();
            var funds = new List<Fund>
            {
                new() { FundId = fundId1, FundName = "Alpha", CurrencyCode = "USD" },
                new() { FundId = fundId2, FundName = "Beta", CurrencyCode = "EUR" }
            };
            
            var mapped = new List<FundReadDto>
            {
                new(fundId1, "Alpha", "USD", DateTime.UtcNow),
                new(fundId2, "Beta", "EUR", DateTime.UtcNow)
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
            var entity = new Fund { FundId = Guid.NewGuid(), FundName = "Alpha", CurrencyCode = "USD" };
            var read = new FundReadDto(entity.FundId, "Alpha", "USD", DateTime.UtcNow);

            _mapper.Setup(m => m.Map<Fund>(dto)).Returns(entity);
            _mapper.Setup(m => m.Map<FundReadDto>(entity)).Returns(read);

            var result = await _service.CreateAsync(dto);

            _fundRepo.Verify(r => r.AddAsync(entity), Times.Once);
            Assert.Equal(read, result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenNotFound()
        {
            var fundId = Guid.NewGuid();
            _fundRepo.Setup(r => r.GetByIdAsync(fundId)).ReturnsAsync((Fund?)null);
            var dto = new FundUpdateDto("New", "EUR", DateTime.UtcNow);

            var result = await _service.UpdateAsync(fundId, dto);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenEntityFound()
        {
            var fundId = Guid.NewGuid();
            var fund = new Fund { FundId = fundId };
            _fundRepo.Setup(r => r.GetByIdAsync(fundId)).ReturnsAsync(fund);

            var result = await _service.DeleteAsync(fundId);

            Assert.True(result);
            _fundRepo.Verify(r => r.DeleteAsync(fund), Times.Once);
        }
    }

}
