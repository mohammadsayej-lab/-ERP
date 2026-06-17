namespace Shared.DTOs;

public class JournalEntryDto
{
    public Guid Id { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public List<JournalEntryDetailDto> Details { get; set; } = new();
}

public class JournalEntryDetailDto
{
    public Guid Id { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
}

public class CreateJournalEntryDto
{
    public string Description { get; set; } = string.Empty;
    public List<CreateJournalEntryDetailDto> Details { get; set; } = new();
}

public class CreateJournalEntryDetailDto
{
    public Guid AccountId { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
}
