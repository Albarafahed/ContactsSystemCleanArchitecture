
using ContactsSystemApplication.DTOs;
using ContactsSystemApplication.Interfaces;

namespace ContactsSystemApplication.Services;

public sealed class AuditLogQueryService(
    IAuditLogRepository auditLogRepository)
    : IAuditLogQueryService
{
    public async Task<AuditLogPagedResultDto> GetPagedAsync(
        int? userId,
        string? action,
        string? entityName,
        DateTime? fromUtc,
        DateTime? toUtc,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (userId.HasValue && userId.Value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(userId),
                "User ID must be greater than zero.");
        }

        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(pageNumber),
                "Page number must be greater than zero.");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(pageSize),
                "Page size must be between 1 and 100.");
        }

        if (fromUtc.HasValue && toUtc.HasValue &&
            fromUtc.Value > toUtc.Value)
        {
            throw new ArgumentException(
                "FromUtc cannot be later than ToUtc.");
        }

        action = NormalizeFilter(action);
        entityName = NormalizeFilter(entityName);

        var (logs, totalCount) =
            await auditLogRepository.GetPagedAsync(
                userId,
                action,
                entityName,
                fromUtc,
                toUtc,
                pageNumber,
                pageSize,
                cancellationToken);

        var items = logs.Select(log => new AuditLogDto
        {
            AuditLogId = log.AuditLogId,
            UserId = log.UserId,
            Action = log.Action,
            EntityName = log.EntityName,
            EntityId = log.EntityId,
            Outcome = log.Outcome,
            TimestampUtc = log.TimestampUtc,
            TraceId = log.TraceId
        }).ToList();

        return new AuditLogPagedResultDto
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize)
        };
    }

    private static string? NormalizeFilter(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}
