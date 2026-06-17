namespace Core.Entities;

public class SalarySlip
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal HousingAllowance { get; set; }
    public decimal TransportAllowance { get; set; }
    public decimal OtherAllowances { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal SocialInsuranceDeduction { get; set; }
    public decimal IncomeTaxDeduction { get; set; }
    public decimal OtherDeductions { get; set; }
    public decimal NetSalary { get; set; }
    public string Status { get; set; } = "Draft"; // Draft, Approved, Paid
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovedBy { get; set; }
}
