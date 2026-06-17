using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Shared.DTOs;
using Shared.Responses;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> GetAllEmployees()
    {
        try
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(ApiResponse<List<EmployeeDto>>.SuccessResponse(employees));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<EmployeeDto>>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetEmployeeById(Guid id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound(ApiResponse<EmployeeDto>.ErrorResponse("Employee not found"));
            return Ok(ApiResponse<EmployeeDto>.SuccessResponse(employee));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<EmployeeDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> CreateEmployee(CreateEmployeeDto dto)
    {
        try
        {
            var employee = await _employeeService.CreateEmployeeAsync(dto);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, 
                ApiResponse<EmployeeDto>.SuccessResponse(employee, "Employee created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<EmployeeDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> UpdateEmployee(Guid id, CreateEmployeeDto dto)
    {
        try
        {
            var employee = await _employeeService.UpdateEmployeeAsync(id, dto);
            return Ok(ApiResponse<EmployeeDto>.SuccessResponse(employee, "Employee updated successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<EmployeeDto>.ErrorResponse("Employee not found"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<EmployeeDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteEmployee(Guid id)
    {
        try
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Employee deleted successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Employee not found"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
}
