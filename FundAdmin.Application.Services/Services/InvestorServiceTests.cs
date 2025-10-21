using Moq;
using AutoMapper;
using FundAdmin.Application.Services;
using FundAdmin.Application.DTOs;
using FundAdmin.Application.Interfaces.Repositories;
using FundAdmin.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FundAdmin.Application.Tests.Services
{
    public class InvestorServiceTests
    {
        private readonly Mock<IInvestorRepository> _investorRepo;
        private readonly Mock<ITransactionRepository> _transactionRepo;
        private readonly Mock<IFundRepository> _fundRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<InvestorService>> _logger;
        private readonly InvestorService _service;

        public InvestorServiceTests()
        {
            _investorRepo = new Mock<IInvestorRepository>();
            _transactionRepo = new Mock<ITransactionRepository>();
            _fundRepo = new Mock<IFundRepository>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<InvestorService>>();
            _service = new InvestorService(_investorRepo.Object, _transactionRepo.Object, _fundRepo.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            // Arrange
            var investors = new List<Investor>
            {
                new() { InvestorId = 1, FullName = "Robert", Email = "robert@test.com", FundId = 1 },
                new() { InvestorId = 2, FullName = "Bob", Email = "bob@test.com", FundId = 2 }
            };
            
            var mapped = new List<InvestorReadDto>
            {
                new(1, "Robert", "robert@test.com", 1),
                new(2, "Bob", "bob@test.com", 2)
            };

            _investorRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(investors);
            _mapper.Setup(m => m.Map<IEnumerable<InvestorReadDto>>(investors)).Returns(mapped);

            var result = await _service.GetAllAsync();

            _investorRepo.Verify(r => r.GetAllAsync(), Times.Once);
            Assert.Equal(mapped, result);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedDto_WhenFundExists()
        {
            var createDto = new InvestorCreateDto("Eric Test", "eric@example.com", 1);
            var entity = new Investor { InvestorId = 1, FullName = "Eric Test", Email = "eric@example.com", FundId = 1 };
            var readDto = new InvestorReadDto(1, "Eric Test", "eric@example.com", 1);

            _fundRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
            _mapper.Setup(m => m.Map<Investor>(createDto)).Returns(entity);
            _mapper.Setup(m => m.Map<InvestorReadDto>(entity)).Returns(readDto);

            var result = await _service.CreateAsync(createDto);

            _investorRepo.Verify(r => r.AddAsync(entity), Times.Once);
            _fundRepo.Verify(r => r.ExistsAsync(1), Times.Once);
            Assert.Equal(readDto, result);            
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenFundDoesNotExist()
        {
            _fundRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(false);
            var dto = new InvestorCreateDto("John", "john@x.com", 1);

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenInvestorExists()
        {
            var investor = new Investor { InvestorId = 1, FullName = "John", Email = "john@x.com", FundId = 2 };

            _investorRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(investor);

            var result = await _service.DeleteAsync(1);

            Assert.True(result);
            _investorRepo.Verify(r => r.DeleteAsync(investor), Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
        {
            _investorRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((Investor?)null);

            var result = await _service.DeleteAsync(5);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenSuccess()
        {
            var dto = new InvestorUpdateDto("Jane", "jane@x.com", 2);
            var entity = new Investor { InvestorId = 1 };

            _fundRepo.Setup(r => r.ExistsAsync(dto.FundId)).ReturnsAsync(true);
            _investorRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            var result = await _service.UpdateAsync(1, dto);

            Assert.True(result);
            _investorRepo.Verify(r => r.UpdateAsync(entity), Times.Once);
        }
    }

}
