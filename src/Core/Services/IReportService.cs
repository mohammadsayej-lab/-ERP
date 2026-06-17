namespace Core.Services;

public interface IReportService
{
    Task<object> GetTrialBalanceAsync();
    Task<object> GetIncomeStatementAsync(int month, int year);
    Task<object> GetBalanceSheetAsync(int month, int year);
    Task<object> GetPayrollSummaryAsync(int month, int year);
}
