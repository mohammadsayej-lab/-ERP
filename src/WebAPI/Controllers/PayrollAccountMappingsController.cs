using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Shared.DTOs;
using Shared.Responses;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayrollAccountMappingsController : ControllerBase
{
    private readonly IPayrollAccountMappingService _mappingService;

    public PayrollAccountMappingsController(IPayrollAccountMappingService mappingService)
    {
        _mappingService = mappingService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PayrollAccountMappingDto>>>> GetAllMappings()
    {
        try
        {
            var mappings = await _mappingService.GetAllMappingsAsync();
            return Ok(ApiResponse<List<PayrollAccountMappingDto>>.SuccessResponse(mappings));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<PayrollAccountMappingDto>>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PayrollAccountMappingDto>>> CreateMapping(CreatePayrollAccountMappingDto dto)
    {
        try
        {
            var mapping = await _mappingService.CreateMappingAsync(dto);
            return CreatedAtAction(nameof(GetAllMappings), 
                ApiResponse<PayrollAccountMappingDto>.SuccessResponse(mapping, "Mapping created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PayrollAccountMappingDto>.ErrorResponse(ex.Message));
        }
    }
}
