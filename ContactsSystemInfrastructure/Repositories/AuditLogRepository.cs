
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using ContactsSystemInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContactsSystemInfrastructure.Repositories;

public sealed class AuditLogRepository(
    ContactsDbContext context) : IAuditLogRepository
{
    public async Task AddAsync(
        AuditLog auditLog,
        CancellationToken cancellationToken = default)
    {
        await context.AuditLogs.AddAsync(
            auditLog,
            cancellationToken);
    }

    public Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<AuditLog> Items, int TotalCount)>
        GetPagedAsync(
            int? userId,
            string? action,
            string? entityName,
            DateTime? fromUtc,
            DateTime? toUtc,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
    {
        IQueryable<AuditLog> query =
            context.AuditLogs.AsNoTracking();

        if (userId.HasValue)
        {
            query = query.Where(
                log => log.UserId == userId.Value);
        }

        if (!string.IsNullOrWhiteSpace(action))
        {
            query = query.Where(
                log => log.Action == action);
        }

        if (!string.IsNullOrWhiteSpace(entityName))
        {
            query = query.Where(
                log => log.EntityName == entityName);
        }

        if (fromUtc.HasValue)
        {
            query = query.Where(
                log => log.TimestampUtc >= fromUtc.Value);
        }

        if (toUtc.HasValue)
        {
            query = query.Where(
                log => log.TimestampUtc <= toUtc.Value);
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderByDescending(log => log.TimestampUtc)
            .ThenByDescending(log => log.AuditLogId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
