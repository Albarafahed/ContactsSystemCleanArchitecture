namespace ContactsSystemApplication.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(
        int? userId,
        string action,
        string entityName,
        string? entityId,
        string outcome,
        string? traceId = null,
        CancellationToken cancellationToken = default);
}