using AutoMapper;
using Core.Entities;
using Shared.DTOs;

namespace Core.MappingProfiles;

public class SalarySlipProfile : Profile
{
    public SalarySlipProfile()
    {
        CreateMap<SalarySlip, SalarySlipDto>();
        CreateMap<CreateSalarySlipDto, SalarySlip>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Draft"));
    }
}
