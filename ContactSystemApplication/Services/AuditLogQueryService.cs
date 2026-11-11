
using ContactsSystemApplication.DTOs;
using ContactsSystemApplication.Interfaces;

namespace ContactsSystemApplication.Services;

public sealed class AuditLogQueryService(
    IAuditLogRepository auditLogRepository)
    : IAuditLogQueryService
{
    public async Task<AuditLogPagedResultDto> GetPagedAsync(
        AuditLogQueryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidateRequest(request);

        var normalizedRequest = new AuditLogQueryRequestDto
        {
            UserId = request.UserId,
            Action = NormalizeFilter(request.Action),
            EntityName = NormalizeFilter(request.EntityName),
            FromUtc = request.FromUtc,
            ToUtc = request.ToUtc,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var (logs, totalCount) =
            await auditLogRepository.GetPagedAsync(
                normalizedRequest,
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
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(
                totalCount / (double)request.PageSize)
        };
    }

    private static void ValidateRequest(
        AuditLogQueryRequestDto request)
    {
        if (request.UserId.HasValue && request.UserId.Value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request.UserId),
                "User ID must be greater than zero.");
        }

        if (request.PageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request.PageNumber),
                "Page number must be greater than zero.");
        }

        if (request.PageSize < 1 || request.PageSize > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(request.PageSize),
                "Page size must be between 1 and 100.");
        }

        if (request.FromUtc.HasValue &&
            request.ToUtc.HasValue &&
            request.FromUtc.Value > request.ToUtc.Value)
        {
            throw new ArgumentException(
                "FromUtc cannot be later than ToUtc.");
        }
    }

    private static string? NormalizeFilter(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}
