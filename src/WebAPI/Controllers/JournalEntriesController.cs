using Microsoft.AspNetCore.Mvc;
using Core.Services;
using Shared.DTOs;
using Shared.Responses;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JournalEntriesController : ControllerBase
{
    private readonly IJournalEntryService _journalEntryService;

    public JournalEntriesController(IJournalEntryService journalEntryService)
    {
        _journalEntryService = journalEntryService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<JournalEntryDto>>>> GetAllEntries()
    {
        try
        {
            var entries = await _journalEntryService.GetAllEntriesAsync();
            return Ok(ApiResponse<List<JournalEntryDto>>.SuccessResponse(entries));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<JournalEntryDto>>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<JournalEntryDto>>> GetEntryById(Guid id)
    {
        try
        {
            var entry = await _journalEntryService.GetEntryByIdAsync(id);
            if (entry == null)
                return NotFound(ApiResponse<JournalEntryDto>.ErrorResponse("Journal entry not found"));
            return Ok(ApiResponse<JournalEntryDto>.SuccessResponse(entry));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<JournalEntryDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<JournalEntryDto>>> CreateEntry(CreateJournalEntryDto dto)
    {
        try
        {
            var entry = await _journalEntryService.CreateEntryAsync(dto);
            return CreatedAtAction(nameof(GetEntryById), new { id = entry.Id }, 
                ApiResponse<JournalEntryDto>.SuccessResponse(entry, "Journal entry created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<JournalEntryDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost("{id}/post")]
    public async Task<ActionResult<ApiResponse<JournalEntryDto>>> PostEntry(Guid id)
    {
        try
        {
            var entry = await _journalEntryService.PostEntryAsync(id);
            return Ok(ApiResponse<JournalEntryDto>.SuccessResponse(entry, "Journal entry posted successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<JournalEntryDto>.ErrorResponse("Journal entry not found"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<JournalEntryDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<JournalEntryDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPost("{id}/reverse")]
    public async Task<ActionResult<ApiResponse<JournalEntryDto>>> ReverseEntry(Guid id)
    {
        try
        {
            var entry = await _journalEntryService.ReverseEntryAsync(id);
            return Ok(ApiResponse<JournalEntryDto>.SuccessResponse(entry, "Journal entry reversed successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<JournalEntryDto>.ErrorResponse("Journal entry not found"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<JournalEntryDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<JournalEntryDto>.ErrorResponse(ex.Message));
        }
    }
}
