namespace Shared.DTOs;

public class SalarySlipDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal HousingAllowance { get; set; }
    public decimal TransportAllowance { get; set; }
    public decimal GrossSalary { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetSalary { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateSalarySlipDto
{
    public Guid EmployeeId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}
