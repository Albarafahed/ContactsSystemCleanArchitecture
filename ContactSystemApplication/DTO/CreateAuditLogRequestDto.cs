namespace ContactsSystemApplication.DTOs;

public sealed class CreateAuditLogRequestDto
{
    public int? UserId { get; init; }
    public required string Action { get; init; }
    public required string EntityName { get; init; }
    public string? EntityId { get; init; }
    public required string Outcome { get; init; }
    public string? TraceId { get; init; }
    public bool SaveImmediately { get; init; }
}