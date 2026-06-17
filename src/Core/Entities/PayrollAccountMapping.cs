namespace Core.Entities;

public class PayrollAccountMapping
{
    public Guid Id { get; set; }
    public string PayrollComponent { get; set; } = string.Empty; // BasicSalary, HousingAllowance, etc.
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string MappingType { get; set; } = string.Empty; // Debit, Credit
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
