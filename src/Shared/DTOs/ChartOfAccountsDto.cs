namespace Shared.DTOs;

public class ChartOfAccountsDto
{
    public Guid Id { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public bool IsActive { get; set; }
}

public class CreateChartOfAccountsDto
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string? Description { get; set; }
}
