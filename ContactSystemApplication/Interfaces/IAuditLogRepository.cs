using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(
        AuditLog auditLog,
        CancellationToken cancellationToken = default);
    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}