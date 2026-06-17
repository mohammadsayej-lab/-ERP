using AutoMapper;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Core.Services;

public class PayrollAccountMappingService : IPayrollAccountMappingService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PayrollAccountMappingService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PayrollAccountMappingDto>> GetAllMappingsAsync()
    {
        var mappings = await _context.PayrollAccountMappings
            .Where(m => m.IsActive)
            .OrderBy(m => m.PayrollComponent)
            .ToListAsync();
        return _mapper.Map<List<PayrollAccountMappingDto>>(mappings);
    }

    public async Task<PayrollAccountMappingDto> CreateMappingAsync(CreatePayrollAccountMappingDto dto)
    {
        // Verify account exists
        var account = await _context.ChartOfAccounts
            .FirstOrDefaultAsync(a => a.AccountCode == dto.AccountCode)
            ?? throw new KeyNotFoundException($"Account code {dto.AccountCode} not found");

        var mapping = new PayrollAccountMapping
        {
            Id = Guid.NewGuid(),
            PayrollComponent = dto.PayrollComponent,
            AccountCode = dto.AccountCode,
            AccountName = dto.AccountName,
            MappingType = dto.MappingType,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.PayrollAccountMappings.Add(mapping);
        await _context.SaveChangesAsync();
        return _mapper.Map<PayrollAccountMappingDto>(mapping);
    }

    public async Task<PayrollAccountMappingDto?> GetMappingByComponentAsync(string component)
    {
        var mapping = await _context.PayrollAccountMappings
            .FirstOrDefaultAsync(m => m.PayrollComponent == component && m.IsActive);
        return mapping == null ? null : _mapper.Map<PayrollAccountMappingDto>(mapping);
    }
}
