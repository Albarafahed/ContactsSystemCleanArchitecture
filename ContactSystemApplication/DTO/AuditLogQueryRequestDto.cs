namespace ContactsSystemApplication.DTOs;

public sealed class AuditLogQueryRequestDto
{
    public int? UserId { get; init; }
    public string? Action { get; init; }
    public string? EntityName { get; init; }
    public DateTime? FromUtc { get; init; }
    public DateTime? ToUtc { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}