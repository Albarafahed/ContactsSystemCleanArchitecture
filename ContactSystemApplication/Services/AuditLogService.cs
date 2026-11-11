using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.AuditLogs.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;

    public AuditLogService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    public async Task LogAsync(
        int? userId,
        string action,
        string entityName,
        string? entityId,
        string outcome,
        string? traceId = null,
        CancellationToken cancellationToken = default)
    {
        var auditLog = new AuditLog(
            userId,
            action,
            entityName,
            entityId,
            outcome,
            traceId);

        await _auditLogRepository.AddAsync(
            auditLog,
            cancellationToken);
    }
}