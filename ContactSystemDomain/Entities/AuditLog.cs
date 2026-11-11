namespace ContactsSystemDomain.Entities;

public class AuditLog
{
    public int AuditLogId { get; private set; }

    public int? UserId { get; private set; }

    public string Action { get; private set; } = null!;

    public string EntityName { get; private set; } = null!;

    public string? EntityId { get; private set; }

    public string Outcome { get; private set; } = null!;

    public DateTime TimestampUtc { get; private set; }

    public string? TraceId { get; private set; }

    private AuditLog()
    {
    }

    public AuditLog(
        int? userId,
        string action,
        string entityName,
        string? entityId,
        string outcome,
        string? traceId)
    {
        UserId = userId;
        Action = action;
        EntityName = entityName;
        EntityId = entityId;
        Outcome = outcome;
        TraceId = traceId;
        TimestampUtc = DateTime.UtcNow;
    }
}