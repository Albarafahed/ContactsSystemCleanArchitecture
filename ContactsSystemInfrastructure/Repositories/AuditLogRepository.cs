using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using ContactsSystemInfrastructure.Persistence;

namespace ContactsSystemInfrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly ContactsDbContext _context;

    public AuditLogRepository(ContactsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        AuditLog auditLog,
        CancellationToken cancellationToken = default)
    {
        await _context.AuditLogs.AddAsync(
            auditLog,
            cancellationToken);
    }
}