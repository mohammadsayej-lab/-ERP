using AutoMapper;
using Core.Entities;
using Shared.DTOs;

namespace Core.MappingProfiles;

public class JournalEntryProfile : Profile
{
    public JournalEntryProfile()
    {
        CreateMap<JournalEntry, JournalEntryDto>();
        CreateMap<JournalEntryDetail, JournalEntryDetailDto>();
    }
}
