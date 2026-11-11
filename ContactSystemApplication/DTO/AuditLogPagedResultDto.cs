
using ContactsSystemApplication.DTOs;

namespace ContactsSystemApplication.DTOs;

public sealed class AuditLogPagedResultDto
{
    public IReadOnlyList<AuditLogDto> Items { get; init; }
        = Array.Empty<AuditLogDto>();

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages { get; init; }
}
