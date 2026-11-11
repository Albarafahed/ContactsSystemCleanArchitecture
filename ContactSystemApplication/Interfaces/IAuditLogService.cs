namespace ContactsSystemApplication.AuditLogs.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(
        int? userId,
        string action,
        string entityName,
        string? entityId,
        string outcome,
        string? traceId = null,
        bool saveImmediately = false,
        CancellationToken cancellationToken = default);
}