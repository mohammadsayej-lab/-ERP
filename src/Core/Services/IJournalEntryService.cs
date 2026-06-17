using Shared.DTOs;

namespace Core.Services;

public interface IJournalEntryService
{
    Task<List<JournalEntryDto>> GetAllEntriesAsync();
    Task<JournalEntryDto?> GetEntryByIdAsync(Guid id);
    Task<JournalEntryDto> CreateEntryAsync(CreateJournalEntryDto dto);
    Task<JournalEntryDto> PostEntryAsync(Guid id);
    Task<JournalEntryDto> ReverseEntryAsync(Guid id);
}
