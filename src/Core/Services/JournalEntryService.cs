using AutoMapper;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.DTOs;

namespace Core.Services;

public class JournalEntryService : IJournalEntryService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public JournalEntryService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<JournalEntryDto>> GetAllEntriesAsync()
    {
        var entries = await _context.JournalEntries
            .Include(je => je.Details)
            .ToListAsync();
        return _mapper.Map<List<JournalEntryDto>>(entries);
    }

    public async Task<JournalEntryDto?> GetEntryByIdAsync(Guid id)
    {
        var entry = await _context.JournalEntries
            .Include(je => je.Details)
            .FirstOrDefaultAsync(je => je.Id == id);
        return entry == null ? null : _mapper.Map<JournalEntryDto>(entry);
    }

    public async Task<JournalEntryDto> CreateEntryAsync(CreateJournalEntryDto dto)
    {
        // Validate that total debit equals total credit
        var totalDebit = dto.Details.Sum(d => d.Debit);
        var totalCredit = dto.Details.Sum(d => d.Credit);

        if (totalDebit != totalCredit)
            throw new InvalidOperationException("Total debit must equal total credit");

        var entry = new JournalEntry
        {
            Id = Guid.NewGuid(),
            EntryNumber = GenerateEntryNumber(),
            EntryDate = DateTime.UtcNow,
            Description = dto.Description,
            Status = EntryStatus.Draft,
            TotalDebit = totalDebit,
            TotalCredit = totalCredit,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System" // Should come from auth context
        };

        foreach (var detail in dto.Details)
        {
            var account = await _context.ChartOfAccounts.FindAsync(detail.AccountId)
                ?? throw new KeyNotFoundException($"Account with ID {detail.AccountId} not found");

            entry.Details.Add(new JournalEntryDetail
            {
                Id = Guid.NewGuid(),
                JournalEntryId = entry.Id,
                AccountId = detail.AccountId,
                AccountCode = account.AccountCode,
                AccountName = account.AccountName,
                Debit = detail.Debit,
                Credit = detail.Credit,
                Description = detail.Description
            });
        }

        _context.JournalEntries.Add(entry);
        await _context.SaveChangesAsync();
        return _mapper.Map<JournalEntryDto>(entry);
    }

    public async Task<JournalEntryDto> PostEntryAsync(Guid id)
    {
        var entry = await _context.JournalEntries
            .Include(je => je.Details)
            .FirstOrDefaultAsync(je => je.Id == id)
            ?? throw new KeyNotFoundException($"Journal entry with ID {id} not found");

        if (entry.IsPosted)
            throw new InvalidOperationException("Journal entry is already posted");

        entry.IsPosted = true;
        entry.Status = EntryStatus.Posted;
        entry.PostedDate = DateTime.UtcNow;
        entry.PostedBy = "System";

        // Update account balances
        foreach (var detail in entry.Details)
        {
            var account = await _context.ChartOfAccounts.FindAsync(detail.AccountId);
            if (account != null)
            {
                account.CurrentBalance += (detail.Debit - detail.Credit);
            }
        }

        await _context.SaveChangesAsync();
        return _mapper.Map<JournalEntryDto>(entry);
    }

    public async Task<JournalEntryDto> ReverseEntryAsync(Guid id)
    {
        var entry = await _context.JournalEntries
            .Include(je => je.Details)
            .FirstOrDefaultAsync(je => je.Id == id)
            ?? throw new KeyNotFoundException($"Journal entry with ID {id} not found");

        if (!entry.IsPosted)
            throw new InvalidOperationException("Only posted entries can be reversed");

        // Create reverse entry
        var reverseEntry = new JournalEntry
        {
            Id = Guid.NewGuid(),
            EntryNumber = GenerateEntryNumber(),
            EntryDate = DateTime.UtcNow,
            Description = $"Reversal of Entry {entry.EntryNumber}",
            Status = EntryStatus.Reversed,
            TotalDebit = entry.TotalCredit,
            TotalCredit = entry.TotalDebit,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        foreach (var detail in entry.Details)
        {
            reverseEntry.Details.Add(new JournalEntryDetail
            {
                Id = Guid.NewGuid(),
                JournalEntryId = reverseEntry.Id,
                AccountId = detail.AccountId,
                AccountCode = detail.AccountCode,
                AccountName = detail.AccountName,
                Debit = detail.Credit,
                Credit = detail.Debit,
                Description = $"Reversal: {detail.Description}"
            });
        }

        _context.JournalEntries.Add(reverseEntry);
        await _context.SaveChangesAsync();
        return _mapper.Map<JournalEntryDto>(reverseEntry);
    }

    private string GenerateEntryNumber()
    {
        var lastEntry = _context.JournalEntries.OrderByDescending(e => e.CreatedAt).FirstOrDefault();
        var number = lastEntry == null ? 1 : int.Parse(lastEntry.EntryNumber) + 1;
        return number.ToString("D6"); // Format as 000001
    }
}
