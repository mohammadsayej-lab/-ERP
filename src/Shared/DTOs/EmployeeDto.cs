namespace Shared.DTOs;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal BasicSalary { get; set; }
    public bool IsActive { get; set; }
}

public class CreateEmployeeDto
{
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal BasicSalary { get; set; }
}
