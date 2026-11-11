
using ContactsSystemApplication.DTOs;
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
       AuditLogQueryRequestDto auditLogQueryRequestDto,
        CancellationToken cancellationToken = default);
}
