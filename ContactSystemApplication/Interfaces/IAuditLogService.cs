using ContactsSystemApplication.DTOs;

namespace ContactsSystemApplication.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(
               CreateAuditLogRequestDto request,
        CancellationToken cancellationToken = default);
}
