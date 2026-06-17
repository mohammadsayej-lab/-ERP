using AutoMapper;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Core.Services;

public class ChartOfAccountsService : IChartOfAccountsService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ChartOfAccountsService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ChartOfAccountsDto>> GetAllAccountsAsync()
    {
        var accounts = await _context.ChartOfAccounts
            .Where(a => a.IsActive)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();
        return _mapper.Map<List<ChartOfAccountsDto>>(accounts);
    }

    public async Task<ChartOfAccountsDto?> GetAccountByIdAsync(Guid id)
    {
        var account = await _context.ChartOfAccounts.FindAsync(id);
        return account == null ? null : _mapper.Map<ChartOfAccountsDto>(account);
    }

    public async Task<ChartOfAccountsDto> CreateAccountAsync(CreateChartOfAccountsDto dto)
    {
        // Check if account code already exists
        var existingAccount = await _context.ChartOfAccounts
            .FirstOrDefaultAsync(a => a.AccountCode == dto.AccountCode);

        if (existingAccount != null)
            throw new InvalidOperationException($"Account code {dto.AccountCode} already exists");

        var account = new ChartOfAccounts
        {
            Id = Guid.NewGuid(),
            AccountCode = dto.AccountCode,
            AccountName = dto.AccountName,
            AccountType = dto.AccountType,
            Description = dto.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.ChartOfAccounts.Add(account);
        await _context.SaveChangesAsync();
        return _mapper.Map<ChartOfAccountsDto>(account);
    }

    public async Task<ChartOfAccountsDto> UpdateAccountAsync(Guid id, CreateChartOfAccountsDto dto)
    {
        var account = await _context.ChartOfAccounts.FindAsync(id)
            ?? throw new KeyNotFoundException($"Account with ID {id} not found");

        // Check if new account code is already used by another account
        if (account.AccountCode != dto.AccountCode)
        {
            var existingAccount = await _context.ChartOfAccounts
                .FirstOrDefaultAsync(a => a.AccountCode == dto.AccountCode && a.Id != id);
            if (existingAccount != null)
                throw new InvalidOperationException($"Account code {dto.AccountCode} already exists");
        }

        account.AccountCode = dto.AccountCode;
        account.AccountName = dto.AccountName;
        account.AccountType = dto.AccountType;
        account.Description = dto.Description;

        await _context.SaveChangesAsync();
        return _mapper.Map<ChartOfAccountsDto>(account);
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        var account = await _context.ChartOfAccounts.FindAsync(id)
            ?? throw new KeyNotFoundException($"Account with ID {id} not found");

        account.IsActive = false;
        await _context.SaveChangesAsync();
    }
}
