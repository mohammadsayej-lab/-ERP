namespace Core.Entities;

public class PayrollProfile
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal HousingAllowance { get; set; }
    public decimal TransportAllowance { get; set; }
    public decimal OtherAllowances { get; set; }
    public decimal SocialInsuranceDeduction { get; set; }
    public decimal IncomeTaxDeduction { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
