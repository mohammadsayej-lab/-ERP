using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Shared.DTOs;
using Shared.Responses;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalarySlipsController : ControllerBase
{
    private readonly ISalarySlipService _salarySlipService;

    public SalarySlipsController(ISalarySlipService salarySlipService)
    {
        _salarySlipService = salarySlipService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SalarySlipDto>>>> GetAllSalarySlips()
    {
        try
        {
            var slips = await _salarySlipService.GetAllSalarySlipsAsync();
            return Ok(ApiResponse<List<SalarySlipDto>>.SuccessResponse(slips));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<SalarySlipDto>>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SalarySlipDto>>> GetSalarySlipById(Guid id)
    {
        try
        {
            var slip = await _salarySlipService.GetSalarySlipByIdAsync(id);
            if (slip == null)
                return NotFound(ApiResponse<SalarySlipDto>.ErrorResponse("Salary slip not found"));
            return Ok(ApiResponse<SalarySlipDto>.SuccessResponse(slip));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<SalarySlipDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SalarySlipDto>>> CreateSalarySlip(CreateSalarySlipDto dto)
    {
        try
        {
            var slip = await _salarySlipService.CreateSalarySlipAsync(dto);
            return CreatedAtAction(nameof(GetSalarySlipById), new { id = slip.Id }, 
                ApiResponse<SalarySlipDto>.SuccessResponse(slip, "Salary slip created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<SalarySlipDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost("{id}/approve")]
    public async Task<ActionResult<ApiResponse<SalarySlipDto>>> ApproveSalarySlip(Guid id)
    {
        try
        {
            var slip = await _salarySlipService.ApproveSalarySlipAsync(id);
            return Ok(ApiResponse<SalarySlipDto>.SuccessResponse(slip, "Salary slip approved successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<SalarySlipDto>.ErrorResponse("Salary slip not found"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<SalarySlipDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost("{id}/post-to-accounting")]
    public async Task<ActionResult<ApiResponse<object>>> PostSalaryToAccounting(Guid id)
    {
        try
        {
            await _salarySlipService.PostSalaryToAccountingAsync(id);
            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Salary slip posted to accounting successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Salary slip not found"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
}
