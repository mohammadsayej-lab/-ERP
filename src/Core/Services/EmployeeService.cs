using AutoMapper;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Core.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EmployeeService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _context.Employees
            .Where(e => e.IsActive)
            .ToListAsync();
        return _mapper.Map<List<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _context.Employees.FindAsync(id);
        return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var employee = _mapper.Map<Employee>(dto);
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> UpdateEmployeeAsync(Guid id, CreateEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id)
            ?? throw new KeyNotFoundException($"Employee with ID {id} not found");

        _mapper.Map(dto, employee);
        employee.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task DeleteEmployeeAsync(Guid id)
    {
        var employee = await _context.Employees.FindAsync(id)
            ?? throw new KeyNotFoundException($"Employee with ID {id} not found");

        employee.IsActive = false;
        employee.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}
