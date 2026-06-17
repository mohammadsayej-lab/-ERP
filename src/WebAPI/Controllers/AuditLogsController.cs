using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Shared.DTOs;
using Shared.Responses;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller}")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditService _auditService;

    public AuditLogsController(IAuditService auditService)
    {
        _auditService = auditService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<object>>>> GetAuditLogs(
        [FromQuery] DateTime? fromDate, 
        [FromQuery] DateTime? toDate,
        [FromQuery] string? entityName,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var logs = await _auditService.GetAuditLogsAsync(fromDate, toDate, entityName, pageNumber, pageSize);
            return Ok(ApiResponse<List<object>>.SuccessResponse(logs));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<object>>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("entity/{entityName}/{entityId}")]
    public async Task<ActionResult<ApiResponse<List<object>>>> GetEntityAuditHistory(
        string entityName,
        Guid entityId)
    {
        try
        {
            var history = await _auditService.GetEntityAuditHistoryAsync(entityName, entityId);
            return Ok(ApiResponse<List<object>>.SuccessResponse(history));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<object>>.ErrorResponse(ex.Message));
        }
    }
}
