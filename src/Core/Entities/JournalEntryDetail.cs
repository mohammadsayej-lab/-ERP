namespace Core.Entities;

public class JournalEntryDetail
{
    public Guid Id { get; set; }
    public Guid JournalEntryId { get; set; }
    public Guid AccountId { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal Debit { get; set; } = 0;
    public decimal Credit { get; set; } = 0;
    public string? Description { get; set; }
    public JournalEntry? JournalEntry { get; set; }
}
