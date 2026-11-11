
using ContactsSystemApplication.DTOs;
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;

namespace ContactsSystemApplication.Services;

public sealed class AuditLogService(
    IAuditLogRepository auditLogRepository)
    : IAuditLogService
{
    public async Task LogAsync(
        CreateAuditLogRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var auditLog = new AuditLog(
            request.UserId,
            request.Action,
            request.EntityName,
            request.EntityId,
            request.Outcome,
            request.TraceId);

        await auditLogRepository.AddAsync(
            auditLog,
            cancellationToken);

        if (request.SaveImmediately)
        {
            await auditLogRepository.SaveChangesAsync(
                cancellationToken);
        }
    }
}
