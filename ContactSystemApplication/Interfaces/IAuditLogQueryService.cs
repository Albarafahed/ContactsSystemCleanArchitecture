
using ContactsSystemApplication.DTOs;

namespace ContactsSystemApplication.Interfaces;

public interface IAuditLogQueryService
{
    Task<AuditLogPagedResultDto> GetPagedAsync(
        int? userId,
        string? action,
        string? entityName,
        DateTime? fromUtc,
        DateTime? toUtc,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
