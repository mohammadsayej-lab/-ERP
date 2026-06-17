using Shared.DTOs;

namespace Core.Services;

public interface IPayrollAccountMappingService
{
    Task<List<PayrollAccountMappingDto>> GetAllMappingsAsync();
    Task<PayrollAccountMappingDto> CreateMappingAsync(CreatePayrollAccountMappingDto dto);
    Task<PayrollAccountMappingDto?> GetMappingByComponentAsync(string component);
}
