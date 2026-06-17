namespace Core.Entities;

public class JournalEntry
{
    public Guid Id { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Draft"; // Draft, Posted, Reversed
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public bool IsPosted { get; set; } = false;
    public string? PostedBy { get; set; }
    public DateTime? PostedDate { get; set; }
    public List<JournalEntryDetail> Details { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
}
