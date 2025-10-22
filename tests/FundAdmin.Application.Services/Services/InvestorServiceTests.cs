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
            var investorId= Guid.NewGuid();
            var fundId1 = Guid.NewGuid();
            var fundId2 = Guid.NewGuid();

            var investors = new List<Investor>
            {
                new() { InvestorId = investorId, FullName = "Robert", Email = "robert@test.com", FundId = fundId1 },
                new() { InvestorId = investorId, FullName = "Bob", Email = "bob@test.com", FundId = fundId2 }
            };
            
            var mapped = new List<InvestorReadDto>
            {
                new(investorId, "Robert", "robert@test.com", fundId1),
                new(investorId, "Bob", "bob@test.com", fundId2)
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
            var investorId = Guid.NewGuid();
            var fundId = Guid.NewGuid();
            var createDto = new InvestorCreateDto("Eric Test", "eric@example.com", fundId);
            var entity = new Investor { InvestorId = investorId, FullName = "Eric Test", Email = "eric@example.com", FundId = fundId };
            var readDto = new InvestorReadDto(investorId, "Eric Test", "eric@example.com", fundId);

            _fundRepo.Setup(r => r.ExistsAsync(fundId)).ReturnsAsync(true);
            _mapper.Setup(m => m.Map<Investor>(createDto)).Returns(entity);
            _mapper.Setup(m => m.Map<InvestorReadDto>(entity)).Returns(readDto);

            var result = await _service.CreateAsync(createDto);

            _investorRepo.Verify(r => r.AddAsync(entity), Times.Once);
            _fundRepo.Verify(r => r.ExistsAsync(fundId), Times.Once);
            Assert.Equal(readDto, result);            
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenFundDoesNotExist()
        {
            var fundId = Guid.NewGuid();
            _fundRepo.Setup(r => r.ExistsAsync(fundId)).ReturnsAsync(false);
            var dto = new InvestorCreateDto("John", "john@x.com", fundId);

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenInvestorExists()
        {
            var investorId = Guid.NewGuid();
            var investor = new Investor { InvestorId = investorId, FullName = "John", Email = "john@x.com", FundId = Guid.NewGuid() };

            _investorRepo.Setup(r => r.GetByIdAsync(investorId)).ReturnsAsync(investor);

            var result = await _service.DeleteAsync(investorId);

            Assert.True(result);
            _investorRepo.Verify(r => r.DeleteAsync(investor), Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
        {
            var investorId = Guid.NewGuid();
            _investorRepo.Setup(r => r.GetByIdAsync(investorId)).ReturnsAsync((Investor?)null);

            var result = await _service.DeleteAsync(investorId);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenSuccess()
        {
            var dto = new InvestorUpdateDto("Jane", "jane@x.com", Guid.NewGuid());
            var entity = new Investor { InvestorId = Guid.NewGuid() };

            _fundRepo.Setup(r => r.ExistsAsync(dto.FundId)).ReturnsAsync(true);
            _investorRepo.Setup(r => r.GetByIdAsync(entity.InvestorId)).ReturnsAsync(entity);

            var result = await _service.UpdateAsync(entity.InvestorId, dto);

            Assert.True(result);
            _investorRepo.Verify(r => r.UpdateAsync(entity), Times.Once);
        }
    }

}
