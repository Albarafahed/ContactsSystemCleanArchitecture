
namespace ContactsSystemApplication.DTOs;

public sealed class AuditLogDto
{
    public int AuditLogId { get; init; }

    public int? UserId { get; init; }

    public string Action { get; init; } = null!;

    public string EntityName { get; init; } = null!;

    public string? EntityId { get; init; }

    public string Outcome { get; init; } = null!;

    public DateTime TimestampUtc { get; init; }

    public string? TraceId { get; init; }
}
