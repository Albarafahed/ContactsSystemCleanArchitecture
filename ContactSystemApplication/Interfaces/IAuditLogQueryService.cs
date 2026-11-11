
using ContactsSystemApplication.DTOs;

namespace ContactsSystemApplication.Interfaces;

public interface IAuditLogQueryService
{
    Task<AuditLogPagedResultDto> GetPagedAsync(
        AuditLogQueryRequestDto auditLogQueryRequestDtom,
        CancellationToken cancellationToken = default);
}
