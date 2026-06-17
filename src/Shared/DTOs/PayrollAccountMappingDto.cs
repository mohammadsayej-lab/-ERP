namespace Shared.DTOs;

public class PayrollAccountMappingDto
{
    public Guid Id { get; set; }
    public string PayrollComponent { get; set; } = string.Empty;
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string MappingType { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreatePayrollAccountMappingDto
{
    public string PayrollComponent { get; set; } = string.Empty;
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string MappingType { get; set; } = string.Empty;
}
