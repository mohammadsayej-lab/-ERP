using Core.Entities;
using Shared.DTOs;

namespace Core.Services;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id);
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<EmployeeDto> UpdateEmployeeAsync(Guid id, CreateEmployeeDto dto);
    Task DeleteEmployeeAsync(Guid id);
}
