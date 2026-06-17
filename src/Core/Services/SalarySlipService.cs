using AutoMapper;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.DTOs;

namespace Core.Services;

public class SalarySlipService : ISalarySlipService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IJournalEntryService _journalEntryService;

    public SalarySlipService(ApplicationDbContext context, IMapper mapper, IJournalEntryService journalEntryService)
    {
        _context = context;
        _mapper = mapper;
        _journalEntryService = journalEntryService;
    }

    public async Task<List<SalarySlipDto>> GetAllSalarySlipsAsync()
    {
        var slips = await _context.SalarySlips
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
        return _mapper.Map<List<SalarySlipDto>>(slips);
    }

    public async Task<SalarySlipDto?> GetSalarySlipByIdAsync(Guid id)
    {
        var slip = await _context.SalarySlips.FindAsync(id);
        return slip == null ? null : _mapper.Map<SalarySlipDto>(slip);
    }

    public async Task<SalarySlipDto> CreateSalarySlipAsync(CreateSalarySlipDto dto)
    {
        var employee = await _context.Employees.FindAsync(dto.EmployeeId)
            ?? throw new KeyNotFoundException($"Employee with ID {dto.EmployeeId} not found");

        var payrollProfile = await _context.PayrollProfiles
            .Where(pp => pp.EmployeeId == dto.EmployeeId && pp.IsActive)
            .FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("No active payroll profile found for this employee");

        // Calculate salary components
        var basicSalary = payrollProfile.BasicSalary;
        var housingAllowance = payrollProfile.HousingAllowance;
        var transportAllowance = payrollProfile.TransportAllowance;
        var otherAllowances = payrollProfile.OtherAllowances;
        var grossSalary = basicSalary + housingAllowance + transportAllowance + otherAllowances;

        var socialInsuranceDeduction = payrollProfile.SocialInsuranceDeduction;
        var incomeTaxDeduction = payrollProfile.IncomeTaxDeduction;
        var otherDeductions = 0m; // Can be configured

        var totalDeductions = socialInsuranceDeduction + incomeTaxDeduction + otherDeductions;
        var netSalary = grossSalary - totalDeductions;

        var slip = new SalarySlip
        {
            Id = Guid.NewGuid(),
            EmployeeId = dto.EmployeeId,
            Month = dto.Month,
            Year = dto.Year,
            BasicSalary = basicSalary,
            HousingAllowance = housingAllowance,
            TransportAllowance = transportAllowance,
            OtherAllowances = otherAllowances,
            GrossSalary = grossSalary,
            SocialInsuranceDeduction = socialInsuranceDeduction,
            IncomeTaxDeduction = incomeTaxDeduction,
            OtherDeductions = otherDeductions,
            NetSalary = netSalary,
            Status = SalarySlipStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };

        _context.SalarySlips.Add(slip);
        await _context.SaveChangesAsync();
        return _mapper.Map<SalarySlipDto>(slip);
    }

    public async Task<SalarySlipDto> ApproveSalarySlipAsync(Guid id)
    {
        var slip = await _context.SalarySlips.FindAsync(id)
            ?? throw new KeyNotFoundException($"Salary slip with ID {id} not found");

        if (slip.Status != SalarySlipStatus.Draft)
            throw new InvalidOperationException("Only draft salary slips can be approved");

        slip.Status = SalarySlipStatus.Approved;
        slip.ApprovedAt = DateTime.UtcNow;
        slip.ApprovedBy = "System"; // Should come from auth context

        await _context.SaveChangesAsync();
        return _mapper.Map<SalarySlipDto>(slip);
    }

    public async Task PostSalaryToAccountingAsync(Guid id)
    {
        var slip = await _context.SalarySlips.FindAsync(id)
            ?? throw new KeyNotFoundException($"Salary slip with ID {id} not found");

        if (slip.Status != SalarySlipStatus.Approved)
            throw new InvalidOperationException("Only approved salary slips can be posted to accounting");

        var employee = await _context.Employees.FindAsync(slip.EmployeeId);
        var mappings = await _context.PayrollAccountMappings
            .Where(m => m.IsActive)
            .ToListAsync();

        var details = new List<CreateJournalEntryDetailDto>();

        // Add salary components
        if (slip.BasicSalary > 0)
        {
            var mapping = mappings.FirstOrDefault(m => m.PayrollComponent == "BasicSalary");
            if (mapping != null)
            {
                var account = await _context.ChartOfAccounts
                    .FirstOrDefaultAsync(a => a.AccountCode == mapping.AccountCode);
                if (account != null)
                {
                    details.Add(new CreateJournalEntryDetailDto
                    {
                        AccountId = account.Id,
                        Debit = slip.BasicSalary,
                        Credit = 0,
                        Description = $"Basic Salary - {employee?.FirstName} {employee?.LastName}"
                    });
                }
            }
        }

        // Add deductions and other components as needed
        // (Similar logic for other components)

        // Create payable account entry
        var payableMapping = mappings.FirstOrDefault(m => m.PayrollComponent == "PayableSalary");
        if (payableMapping != null)
        {
            var payableAccount = await _context.ChartOfAccounts
                .FirstOrDefaultAsync(a => a.AccountCode == payableMapping.AccountCode);
            if (payableAccount != null)
            {
                details.Add(new CreateJournalEntryDetailDto
                {
                    AccountId = payableAccount.Id,
                    Debit = 0,
                    Credit = slip.NetSalary,
                    Description = $"Payable Salary - {employee?.FirstName} {employee?.LastName}"
                });
            }
        }

        // Create journal entry
        if (details.Count >= 2)
        {
            var journalDto = new CreateJournalEntryDto
            {
                Description = $"Salary posting for {slip.Month}/{slip.Year}",
                Details = details
            };

            await _journalEntryService.CreateEntryAsync(journalDto);
        }

        slip.Status = SalarySlipStatus.Paid;
        await _context.SaveChangesAsync();
    }
}
