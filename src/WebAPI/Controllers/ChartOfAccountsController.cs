using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Shared.DTOs;
using Shared.Responses;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChartOfAccountsController : ControllerBase
{
    private readonly IChartOfAccountsService _chartOfAccountsService;

    public ChartOfAccountsController(IChartOfAccountsService chartOfAccountsService)
    {
        _chartOfAccountsService = chartOfAccountsService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ChartOfAccountsDto>>>> GetAllAccounts()
    {
        try
        {
            var accounts = await _chartOfAccountsService.GetAllAccountsAsync();
            return Ok(ApiResponse<List<ChartOfAccountsDto>>.SuccessResponse(accounts));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<ChartOfAccountsDto>>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ChartOfAccountsDto>>> GetAccountById(Guid id)
    {
        try
        {
            var account = await _chartOfAccountsService.GetAccountByIdAsync(id);
            if (account == null)
                return NotFound(ApiResponse<ChartOfAccountsDto>.ErrorResponse("Account not found"));
            return Ok(ApiResponse<ChartOfAccountsDto>.SuccessResponse(account));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<ChartOfAccountsDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ChartOfAccountsDto>>> CreateAccount(CreateChartOfAccountsDto dto)
    {
        try
        {
            var account = await _chartOfAccountsService.CreateAccountAsync(dto);
            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, 
                ApiResponse<ChartOfAccountsDto>.SuccessResponse(account, "Account created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<ChartOfAccountsDto>.ErrorResponse(ex.Message));
        }
    }
}
