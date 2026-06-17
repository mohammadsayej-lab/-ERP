using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Shared.DTOs;
using Shared.Responses;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller}")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("trial-balance")]
    public async Task<ActionResult<ApiResponse<object>>> GetTrialBalance()
    {
        try
        {
            var trialBalance = await _reportService.GetTrialBalanceAsync();
            return Ok(ApiResponse<object>.SuccessResponse(trialBalance));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("income-statement")]
    public async Task<ActionResult<ApiResponse<object>>> GetIncomeStatement(int month, int year)
    {
        try
        {
            var incomeStatement = await _reportService.GetIncomeStatementAsync(month, year);
            return Ok(ApiResponse<object>.SuccessResponse(incomeStatement));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("balance-sheet")]
    public async Task<ActionResult<ApiResponse<object>>> GetBalanceSheet(int month, int year)
    {
        try
        {
            var balanceSheet = await _reportService.GetBalanceSheetAsync(month, year);
            return Ok(ApiResponse<object>.SuccessResponse(balanceSheet));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("payroll-summary")]
    public async Task<ActionResult<ApiResponse<object>>> GetPayrollSummary(int month, int year)
    {
        try
        {
            var summary = await _reportService.GetPayrollSummaryAsync(month, year);
            return Ok(ApiResponse<object>.SuccessResponse(summary));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
}
