using Shared.DTOs;

namespace Core.Services;

public interface IChartOfAccountsService
{
    Task<List<ChartOfAccountsDto>> GetAllAccountsAsync();
    Task<ChartOfAccountsDto?> GetAccountByIdAsync(Guid id);
    Task<ChartOfAccountsDto> CreateAccountAsync(CreateChartOfAccountsDto dto);
    Task<ChartOfAccountsDto> UpdateAccountAsync(Guid id, CreateChartOfAccountsDto dto);
    Task DeleteAccountAsync(Guid id);
}
