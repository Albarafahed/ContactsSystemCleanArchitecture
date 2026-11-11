using ContactsSystemApplication.AuditLogs.Interfaces;
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.AuditLogs.Services;

public sealed class AuditLogService(
    IAuditLogRepository auditLogRepository) : IAuditLogService
{
    public async Task LogAsync(
        int? userId,
        string action,
        string entityName,
        string? entityId,
        string outcome,
        string? traceId = null,
        bool saveImmediately = false,
        CancellationToken cancellationToken = default)
    {
        var auditLog = new AuditLog(
            userId,
            action,
            entityName,
            entityId,
            outcome,
            traceId);

        await auditLogRepository.AddAsync(
            auditLog,
            cancellationToken);

        if (saveImmediately)
        {
            await auditLogRepository.SaveChangesAsync(
                cancellationToken);
        }
    }
}