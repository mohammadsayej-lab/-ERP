using Core.Entities;

namespace Core.Services;

public interface IAuditService
{
    Task LogActionAsync(string entityName, Guid entityId, string action, string? oldValues, string? newValues, string userId, string? ipAddress = null, string? userAgent = null);
    Task<List<object>> GetAuditLogsAsync(DateTime? fromDate, DateTime? toDate, string? entityName, int pageNumber = 1, int pageSize = 50);
    Task<List<object>> GetEntityAuditHistoryAsync(string entityName, Guid entityId);
}
