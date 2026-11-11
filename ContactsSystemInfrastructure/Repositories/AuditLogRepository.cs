
using ContactsSystemApplication.DTOs;
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
          AuditLogQueryRequestDto queryRequestDto,
            CancellationToken cancellationToken = default)
    {
        IQueryable<AuditLog> query =
            context.AuditLogs.AsNoTracking();

        if (queryRequestDto.UserId.HasValue)
        {
            query = query.Where(
                log => log.UserId == queryRequestDto.UserId.Value);
        }

        if (!string.IsNullOrWhiteSpace(queryRequestDto.Action))
        {
            query = query.Where(
                log => log.Action == queryRequestDto.Action);
        }

        if (!string.IsNullOrWhiteSpace(queryRequestDto.EntityName))
        {
            query = query.Where(
                log => log.EntityName == queryRequestDto.EntityName);
        }

        if (queryRequestDto.FromUtc.HasValue)
        {
            query = query.Where(
                log => log.TimestampUtc >= queryRequestDto.FromUtc.Value);
        }

        if (queryRequestDto.ToUtc.HasValue)
        {
            query = query.Where(
                log => log.TimestampUtc <= queryRequestDto.ToUtc.Value);
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderByDescending(log => log.TimestampUtc)
            .ThenByDescending(log => log.AuditLogId)
            .Skip((queryRequestDto.PageNumber - 1) * queryRequestDto.PageSize)
            .Take(queryRequestDto.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
