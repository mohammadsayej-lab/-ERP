using AutoMapper;
using Core.Entities;
using Shared.DTOs;

namespace Core.MappingProfiles;

public class PayrollAccountMappingProfile : Profile
{
    public PayrollAccountMappingProfile()
    {
        CreateMap<PayrollAccountMapping, PayrollAccountMappingDto>();
        CreateMap<CreatePayrollAccountMappingDto, PayrollAccountMapping>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
