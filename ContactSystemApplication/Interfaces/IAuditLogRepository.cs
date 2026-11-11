
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(
        AuditLog auditLog,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<AuditLog> Items, int TotalCount)> GetPagedAsync(
        int? userId,
        string? action,
        string? entityName,
        DateTime? fromUtc,
        DateTime? toUtc,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
