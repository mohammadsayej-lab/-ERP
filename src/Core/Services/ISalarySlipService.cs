using Shared.DTOs;

namespace Core.Services;

public interface ISalarySlipService
{
    Task<List<SalarySlipDto>> GetAllSalarySlipsAsync();
    Task<SalarySlipDto?> GetSalarySlipByIdAsync(Guid id);
    Task<SalarySlipDto> CreateSalarySlipAsync(CreateSalarySlipDto dto);
    Task<SalarySlipDto> ApproveSalarySlipAsync(Guid id);
    Task PostSalaryToAccountingAsync(Guid id);
}
