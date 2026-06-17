using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;

namespace Core.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetTrialBalanceAsync()
    {
        var accounts = await _context.ChartOfAccounts
            .Where(a => a.IsActive)
            .OrderBy(a => a.AccountCode)
            .Select(a => new
            {
                a.AccountCode,
                a.AccountName,
                a.AccountType,
                Debit = a.CurrentBalance > 0 ? a.CurrentBalance : 0,
                Credit = a.CurrentBalance < 0 ? Math.Abs(a.CurrentBalance) : 0,
                a.CurrentBalance
            })
            .ToListAsync();

        var totalDebit = accounts.Sum(a => (decimal)a.Debit);
        var totalCredit = accounts.Sum(a => (decimal)a.Credit);

        return new
        {
            Title = "Trial Balance",
            GeneratedDate = DateTime.UtcNow,
            Accounts = accounts,
            Summary = new
            {
                TotalDebit = totalDebit,
                TotalCredit = totalCredit,
                IsBalanced = totalDebit == totalCredit
            }
        };
    }

    public async Task<object> GetIncomeStatementAsync(int month, int year)
    {
        var entries = await _context.JournalEntries
            .Include(je => je.Details)
            .Where(je => je.IsPosted && je.EntryDate.Month == month && je.EntryDate.Year == year)
            .ToListAsync();

        var revenues = entries
            .SelectMany(je => je.Details)
            .Where(d => d.AccountName.Contains("Revenue"))
            .GroupBy(d => d.AccountName)
            .Select(g => new { Account = g.Key, Amount = g.Sum(d => d.Credit) })
            .ToList();

        var expenses = entries
            .SelectMany(je => je.Details)
            .Where(d => d.AccountName.Contains("Expense"))
            .GroupBy(d => d.AccountName)
            .Select(g => new { Account = g.Key, Amount = g.Sum(d => d.Debit) })
            .ToList();

        var totalRevenue = revenues.Sum(r => r.Amount);
        var totalExpenses = expenses.Sum(e => e.Amount);
        var netIncome = totalRevenue - totalExpenses;

        return new
        {
            Title = "Income Statement",
            Period = $"{month:D2}/{year}",
            GeneratedDate = DateTime.UtcNow,
            Revenues = revenues,
            Expenses = expenses,
            Summary = new
            {
                TotalRevenue = totalRevenue,
                TotalExpenses = totalExpenses,
                NetIncome = netIncome
            }
        };
    }

    public async Task<object> GetBalanceSheetAsync(int month, int year)
    {
        var accounts = await _context.ChartOfAccounts
            .Where(a => a.IsActive && (a.AccountType == AccountTypes.Asset || 
                                        a.AccountType == AccountTypes.Liability || 
                                        a.AccountType == AccountTypes.Equity))
            .ToListAsync();

        var assets = accounts
            .Where(a => a.AccountType == AccountTypes.Asset)
            .Select(a => new { a.AccountCode, a.AccountName, Amount = a.CurrentBalance })
            .ToList();

        var liabilities = accounts
            .Where(a => a.AccountType == AccountTypes.Liability)
            .Select(a => new { a.AccountCode, a.AccountName, Amount = Math.Abs(a.CurrentBalance) })
            .ToList();

        var equity = accounts
            .Where(a => a.AccountType == AccountTypes.Equity)
            .Select(a => new { a.AccountCode, a.AccountName, Amount = a.CurrentBalance })
            .ToList();

        var totalAssets = assets.Sum(a => a.Amount);
        var totalLiabilities = liabilities.Sum(l => l.Amount);
        var totalEquity = equity.Sum(e => e.Amount);

        return new
        {
            Title = "Balance Sheet",
            AsOfDate = $"{month:D2}/{year}",
            GeneratedDate = DateTime.UtcNow,
            Assets = assets,
            Liabilities = liabilities,
            Equity = equity,
            Summary = new
            {
                TotalAssets = totalAssets,
                TotalLiabilities = totalLiabilities,
                TotalEquity = totalEquity,
                IsBalanced = totalAssets == (totalLiabilities + totalEquity)
            }
        };
    }

    public async Task<object> GetPayrollSummaryAsync(int month, int year)
    {
        var salarySlips = await _context.SalarySlips
            .Where(s => s.Month == month && s.Year == year)
            .ToListAsync();

        var summary = new
        {
            Title = "Payroll Summary",
            Period = $"{month:D2}/{year}",
            GeneratedDate = DateTime.UtcNow,
            EmployeeCount = salarySlips.Count,
            TotalBasicSalary = salarySlips.Sum(s => s.BasicSalary),
            TotalAllowances = salarySlips.Sum(s => s.HousingAllowance + s.TransportAllowance + s.OtherAllowances),
            TotalGrossSalary = salarySlips.Sum(s => s.GrossSalary),
            TotalDeductions = salarySlips.Sum(s => s.SocialInsuranceDeduction + s.IncomeTaxDeduction + s.OtherDeductions),
            TotalNetSalary = salarySlips.Sum(s => s.NetSalary),
            AverageNetSalary = salarySlips.Count > 0 ? salarySlips.Sum(s => s.NetSalary) / salarySlips.Count : 0,
            StatusBreakdown = new
            {
                Draft = salarySlips.Count(s => s.Status == "Draft"),
                Approved = salarySlips.Count(s => s.Status == "Approved"),
                Paid = salarySlips.Count(s => s.Status == "Paid")
            }
        };

        return summary;
    }
}
