using AutoMapper;
using FundAdmin.Application.DTOs;
using FundAdmin.Domain.Entities;

namespace FundAdmin.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Fund, FundReadDto>();
        CreateMap<FundCreateDto, Fund>();
        CreateMap<FundUpdateDto, Fund>();

        CreateMap<Investor, InvestorReadDto>();
        CreateMap<InvestorCreateDto, Investor>();
        CreateMap<InvestorUpdateDto, Investor>();

        CreateMap<Transaction, TransactionReadDto>();
        CreateMap<TransactionCreateDto, Transaction>();
    }
}
