using AutoMapper;
using Core.Entities;
using Shared.DTOs;

namespace Core.MappingProfiles;

public class ChartOfAccountsProfile : Profile
{
    public ChartOfAccountsProfile()
    {
        CreateMap<ChartOfAccounts, ChartOfAccountsDto>();
        CreateMap<CreateChartOfAccountsDto, ChartOfAccounts>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
